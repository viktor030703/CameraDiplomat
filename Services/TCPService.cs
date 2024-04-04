using System.Net.Sockets;

namespace CameraDiplomat.Services
{
	public class TCPService
	{
		public delegate void NewMessageGetter(string message);
		public delegate void MessageSenderFeedback(string message);
		public delegate void MesssageCameraUnavalible();

		public event NewMessageGetter EventNewMessageGet;
		public event NewMessageGetter EventSenderFeedback;
		public event MesssageCameraUnavalible EventCameraUnavalible;

		private SemaphoreSlim _readLock = new SemaphoreSlim(1, 1);


		//Взять с сервиса!
		private ConfigurationService _configurationService;
		public string CameraIP;
		public int CameraPort;
		//Взять с сервиса!

		private TcpClient _tcpClient;
		private StreamReader _reader;
		private StreamWriter _writer;
		private NetworkStream stream;


		private int millisecondsToTimerEplisted = 500;

		private System.Timers.Timer timer = new System.Timers.Timer();
		private int interval = 500;

		public void StartTimer()
		{
			timer.Interval = interval;
			timer.AutoReset = true;
			timer.Start();
			timer.Elapsed += (e, args) => MonitorIfCameraConnectedByTimer();
		}

		public TCPService(ConfigurationService service)
		{
			_configurationService = service;
			CameraIP = service.CameraIP;
			CameraPort = service.CameraPort;



			ConnectToCamera();


		}


		public void MonitorIfCameraConnectedByTimer()
		{
			//if (_tcpClient.)
			//EventCameraUnavalible.Invoke();
		}


		public void EventCheckCameraStatus()
		{
			if (_tcpClient == null || _reader == null || _writer == null)
			{
				EventCameraUnavalible?.Invoke();
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

				_tcpClient = new TcpClient(CameraIP, CameraPort);

				if (_tcpClient.Connected)
				{
					stream = _tcpClient.GetStream();
					_reader = new StreamReader(stream);
					_writer = new StreamWriter(stream);


				}
				else
				{
					EventCameraUnavalible?.Invoke();
				}
			}
			catch (SocketException ex)
			{
				EventCameraUnavalible?.Invoke();
			}
			catch (Exception E)
			{
				EventCameraUnavalible?.Invoke();
			}
		}

		public void ConnectToCamera(string IP, int port)
		{


			try
			{
				//Disconnect();
				_tcpClient = new TcpClient(IP, port);

				if (_tcpClient.Connected)
				{
					stream = _tcpClient.GetStream();
					_reader = new StreamReader(stream);
					_writer = new StreamWriter(stream);
				}
				else
				{
					EventCameraUnavalible?.Invoke();
				}
			}
			catch (SocketException ex)
			{
				EventCameraUnavalible?.Invoke();
			}
			catch (Exception E)
			{
				EventCameraUnavalible?.Invoke();
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
							await _readLock.WaitAsync();

							messageFromServer = await _reader.ReadLineAsync();
							if (!String.IsNullOrEmpty(messageFromServer))
							{
								EventNewMessageGet?.Invoke(messageFromServer);
							}
							Thread.Sleep(1000);
						}
						catch (Exception E)
						{

						}
						finally
						{
							_readLock.Release();
						}
					}
					else
					{
						Disconnect();
						EventCameraUnavalible?.Invoke();
						break;
					}
				}
			}
		}

		public async Task SendMessage(string message)
		{
			if (_tcpClient == null & _reader != null & _writer != null)
			{
				try
				{
					await _writer.WriteAsync(message);
					await _writer.FlushAsync();
					EventSenderFeedback("ok");
				}
				catch (Exception ex)
				{
					EventCameraUnavalible?.Invoke();
				}
			}
			else
			{
				EventCameraUnavalible?.Invoke();
			}
		}

		public void Disconnect()
		{
			_tcpClient?.Close();
			_reader?.Close();
			_writer?.Close();

		}
	}
}
