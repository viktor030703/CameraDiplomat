using System.Net.Sockets;
using CameraDiplomat.Interfaces;
using Serilog;

namespace CameraDiplomat.Services
{
	public class TCPService : ITCPService
	{
		private readonly ConfigurationService _configurationService;

		public delegate void NewMessageGetter(string message);
		public delegate void MesssageCameraUnavalible();
		public delegate void CameraConnectedSuccessfully();

		public event NewMessageGetter EventNewMessageGet;
		public event MesssageCameraUnavalible EventCameraUnavalible;
		public event CameraConnectedSuccessfully EventCameraConnectedSuccessfully;

		private SemaphoreSlim _semaphoreRead;
		private SemaphoreSlim _semaphoreWrite;

		private TcpClient _tcpClient;
		private StreamReader _reader;
		private StreamWriter _writer;
		private NetworkStream stream;

		public TCPService(ConfigurationService service)
		{
			_configurationService = service;
			_semaphoreRead = new SemaphoreSlim(1, 1);
			_semaphoreWrite = new SemaphoreSlim(1, 1);
			Task.Run(ConnectToCamera);
			//ConnectToCamera();
		}
		public void CameraDisconnectHandler()
		{
			_tcpClient?.Close();
			_reader?.Close();
			_writer?.Close();
			_configurationService.IsCameraConnected = false;
			EventCameraUnavalible?.Invoke();
			Log.Information("Камера отключена");
			Log.CloseAndFlush();
		}
		public void CameraConnectHandler()
		{
			_configurationService.IsCameraConnected = true;
			EventCameraConnectedSuccessfully?.Invoke();
			Log.Information("Камера успешно подключена");
			Log.CloseAndFlush();
		}
		public bool BooleanCheckCameraStatus()
		{
			if (_tcpClient == null || _reader == null || _writer == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		public void ConnectToCamera()
		{
			try
			{
				_tcpClient = new TcpClient(_configurationService.CameraIP, _configurationService.CameraPort);
				if (_tcpClient.Connected)
				{
					stream = _tcpClient.GetStream();
					_reader = new StreamReader(stream);
					_writer = new StreamWriter(stream);
					CameraConnectHandler();
					Task.Run(GetMessages);
				}
				else
				{
					CameraDisconnectHandler();
				}
			}
			catch (SocketException ex)
			{
				Log.Error("Исключение во время подключения камеры:" + ex.Message.ToString());
				CameraDisconnectHandler();
			}
			catch (Exception ex)
			{
				Log.Error("Исключение во время подключения камеры:" + ex.Message.ToString());
				CameraDisconnectHandler();
			}
		}
		public async void GetMessages()
		{
			string? messageFromServer;
			{
				while (true)
				{
					if (_reader != null & _writer != null)
					{
						try
						{
							await _semaphoreRead.WaitAsync();
							messageFromServer = await _reader.ReadLineAsync();
							if (!String.IsNullOrEmpty(messageFromServer))
							{
								EventNewMessageGet?.Invoke(messageFromServer);
							}
							Thread.Sleep(10);
						}
						catch (Exception ex)
						{
							Log.Error("Исключение во время получения сообщения от камеры:" + ex.Message.ToString());
							CameraDisconnectHandler();
							break;
						}
						finally
						{
							_semaphoreRead?.Release();
						}
					}
					else
					{
						Log.Error("Ошибка получения сообщени: потоки чтения и записи не существуют");
						CameraDisconnectHandler();
						break;
					}
				}
			}
		}
		public async Task<bool> SendMessage(string message)
		{
			string response = String.Empty;

			if (_tcpClient != null & _reader != null & _writer != null)
			{
				try
				{
					await _semaphoreWrite.WaitAsync();
					await _writer.WriteLineAsync(message);
					await _writer.FlushAsync();
					Log.Information("Сообщение " + message + " отправлено на камеру");
					_semaphoreWrite.Release();
					return true;
				}
				catch (Exception ex)
				{
					Log.Error("Исключение во время отправки сообщения от камеры:" + ex.Message.ToString());
					CameraDisconnectHandler();
					_semaphoreWrite.Release();
					return false;
				}
			}
			else
			{
				Log.Error("Ошибка отправки сообщени: потоки чтения и записи не существуют");
				CameraDisconnectHandler();
				return false;
			}
		}


		public void StopSemaphores()
		{
			_semaphoreRead.WaitAsync();
			_semaphoreWrite.WaitAsync();
		}

		public void ResumeSemaphores()
		{
			_semaphoreRead.Release();
			_semaphoreWrite.Release();
		}





		public async Task<string> SendMessageAndGetResponce(string message)
		{
			string response = String.Empty;

			if (_tcpClient != null & _reader != null & _writer != null)
			{
				try
				{
					await _writer.WriteLineAsync(message);
					await _writer.FlushAsync();
					Log.Information("Сообщение " + message + " отправлено на камеру");
					response = await _reader.ReadLineAsync();
					Log.Error("Ответ на команду: " + response);
					EventNewMessageGet?.Invoke(response);
					return response;
				}
				catch (Exception ex)
				{
					Log.Error("Исключение во время отправки сообщения от камеры:" + ex.Message.ToString());
					CameraDisconnectHandler();
					return "error";
				}
			}
			else
			{
				Log.Error("Ошибка отправки сообщени: потоки чтения и записи не существуют");
				CameraDisconnectHandler();
				return "error";
			}
		}

	}
}