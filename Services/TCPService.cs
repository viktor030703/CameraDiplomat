using System.Net.Sockets;

namespace CameraDiplomat.Services
{
	public class TCPService
	{
		public delegate void MessageReceivedHandler(string message);
		public event MessageReceivedHandler MessageReceived;

		public void ReceiveMessage(string message)
		{
			// Вызов события
			MessageReceived?.Invoke(message);
		}



		//Взять с сервиса!
		public string CameraIP = "127.0.0.3";
		public int CameraPort = 8080;
		//Взять с сервиса!

		private TcpClient _tcpClient;
		private StreamReader _reader;
		private StreamWriter _writer;
		private NetworkStream stream;
		public TCPService()
		{
			try
			{
				while (true)
				{

					_tcpClient = new TcpClient(CameraIP, CameraPort);

					if (_tcpClient.Connected)
					{
						stream = _tcpClient.GetStream();
						_reader = new StreamReader(stream);
						_writer = new StreamWriter(stream);
						break;
					}
					else
					{
						ReceiveMessage("Camera is unavalible");
					}
				}
			}
			catch (Exception E)
			{
				ReceiveMessage("Camera is unavalible");
			}
		}


		public async void GetMessages()
		{
			string? messageFromServer;
			if (_reader != null & _writer != null)
			{

				while (true)
				{



					messageFromServer = await _reader.ReadToEndAsync();
					if (!String.IsNullOrEmpty(messageFromServer))
					{
						//
						ReceiveMessage(messageFromServer);
						//ReceiveMessage(messageFromServer);
						//Console.WriteLine(messageFromServer);
						//вывод сообщения
						//Console.WriteLine(MessegeDiplomat("<start>Quality<is>false<percent>50<codeIs>123456789abcdef<end>"));

					}
					Thread.Sleep(10);
				}
			}
			else
			{
				ReceiveMessage("Камера недоступна");
			}
		}

		public async Task<string> SendMessage(string message)
		{
			if (_reader != null & _writer != null)
			{
				try
				{
					await _writer.WriteAsync(message);
					await _writer.FlushAsync();
					return "message sended sucessfully!";
				}
				catch (Exception ex)
				{
					return ex.Message;
				}
			}
			else
			{
				return "камера недоступна";
			}
		}
	}
}
