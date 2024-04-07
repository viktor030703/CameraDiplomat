using System.Net.Sockets;
using System.Text;

namespace CameraDiplomat.Services
{
	public class TCPService
	{
		public delegate void NewMessageGetter(string message);
		public delegate void MesssageCameraUnavalible();
		public delegate void CameraConnectedSuccessfully();

		public event NewMessageGetter EventNewMessageGet;
		public event MesssageCameraUnavalible EventCameraUnavalible;
		public event CameraConnectedSuccessfully EventCameraConnectedSuccessfully;

		private SemaphoreSlim _readLock = new SemaphoreSlim(1, 1);

		private ConfigurationService _configurationService;

		private TcpClient _tcpClient;
		private StreamReader _reader;
		private StreamWriter _writer;
		private NetworkStream stream;


		private System.Timers.Timer timer = new System.Timers.Timer();


		public TCPService(ConfigurationService service)
		{
			_configurationService = service;
			ConnectToCamera();
			TimerInitialization();
		}

		public void CameraDisconnectHandler()
		{
			_configurationService.IsCameraConnected = false;
			EventCameraUnavalible?.Invoke();
			Disconnect();
		}

		public void CameraConnectHandler()
		{
			_configurationService.IsCameraConnected = true;
			EventCameraConnectedSuccessfully?.Invoke();
		}

		private void TimerInitialization()
		{
			timer.Interval = _configurationService.tcpTimerInterval;
			timer.AutoReset = true;
			timer.Elapsed += (e, args) => MonitorIfCameraConnectedByTimer();
		}
		public void StartTimer()
		{
			timer.Start();
		}

		public void MonitorIfCameraConnectedByTimer()
		{
			if (_tcpClient == null || _reader == null || _writer == null)
			{
				try
				{
					_writer.WriteLine(".");
					_writer.Flush();
				}
				catch (SocketException ex)
				{
					CameraDisconnectHandler();

				}
				catch (Exception ex)
				{
					CameraDisconnectHandler();
				}
			}
			else
			{
				CameraDisconnectHandler();
			}
		}

		public void CheckConnectionNotNull()
		{
			if (_tcpClient == null || _reader == null || _writer == null)
			{
				CameraDisconnectHandler();
			}
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
				}
				else
				{
					CameraDisconnectHandler();
				}
			}
			catch (SocketException ex)
			{
				CameraDisconnectHandler();
			}
			catch (Exception E)
			{
				CameraDisconnectHandler();
			}
		}

		public void ConnectToCamera(string IP, int port)
		{
			try
			{
				_tcpClient = new TcpClient(IP, port);

				if (_tcpClient.Connected)
				{
					stream = _tcpClient.GetStream();
					_reader = new StreamReader(stream);
					_writer = new StreamWriter(stream);
					CameraConnectHandler();
				}
				else
				{
					CameraDisconnectHandler();
				}
			}
			catch (SocketException ex)
			{
				CameraDisconnectHandler();
			}
			catch (Exception E)
			{
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
							//await _readLock.WaitAsync();
							messageFromServer = await _reader.ReadLineAsync();
							if (!String.IsNullOrEmpty(messageFromServer))
							{
								EventNewMessageGet?.Invoke(messageFromServer);
							}
							Thread.Sleep(10);
						}
						catch (Exception E)
						{

						}
						finally
						{
							//_readLock.Release();
						}
					}
					else
					{
						CameraDisconnectHandler();
						break;
					}
				}
			}
		}


		public async Task CustomPingServer()
		{
			//if(_tcpClient != null)
			//{
			//	byte[] buf = Encoding.ASCII.GetBytes("ping");
			//	_writer.Write('', 0, buf.Length);

			//}
			//else
			//{
			//	CameraConnectHandler();
			//}
		}
		
		public async Task<string> SendMessage(string message)
		{
			string response = String.Empty;
			if (_tcpClient == null & _reader != null & _writer != null)
			{
				try
				{
					await _writer.WriteAsync(message);
					await _writer.FlushAsync();
					response = await _reader.ReadLineAsync();
				}
				catch (Exception ex)
				{
					CameraDisconnectHandler();
				}
			}
			else
			{
				CameraDisconnectHandler();
			}
			return response;
		}

		public void Disconnect()
		{
			_tcpClient?.Close();
			_reader?.Close();
			_writer?.Close();

		}
	}
}
