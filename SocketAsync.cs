using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

// State object for receiving data from remote device.
public class StateObject {
    // Client socket.
    public Socket workSocket = null;
    // Size of receive buffer.
    public const int BufferSize = 256;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
}

public class SocketAsync {
    // The port number for the remote device.
    private static int hostPort = 3005;

    // ManualResetEvent instances signal completion.
    private static ManualResetEvent connectDone =
        new ManualResetEvent(false);
    private static ManualResetEvent sendDone =
        new ManualResetEvent(false);
    private static ManualResetEvent receiveDone =
        new ManualResetEvent(false);

    // The response from the remote device.
    public static String response = String.Empty;

    public static Socket SocketInit(String hostAddress, int port) {
        // Connect to a remote device.
        try {

			hostPort = port;
            // Establish the remote endpoint for the socket.
            // The name of the
            // remote device is "host.contoso.com".
            IPHostEntry ipHostInfo = Dns.Resolve(hostAddress);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.
            Socket socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.
            socket.BeginConnect( remoteEP,
                new AsyncCallback(ConnectCallback), socket);

            // EXAMPLE Receive the response from the remote device.
            //Receive(socket);

			return socket;

        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }

		return null;
    }

	public static void SocketClose(Socket socket)
	{
		// Release the socket.
		socket.Shutdown(SocketShutdown.Both);
        socket.Close();
	}

    private static void ConnectCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.
            Socket socket = (Socket) ar.AsyncState;

            // Complete the connection.
            socket.EndConnect(ar);

            Console.WriteLine("Socket connected to {0}",
                socket.RemoteEndPoint.ToString());

            // Signal that the connection has been made.
            connectDone.Set();
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    public static void Receive(Socket socket) {
        try {
            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = socket;

            // Begin receiving the data from the remote device.
            socket.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    private static void ReceiveCallback( IAsyncResult ar ) {
        try {
            // Retrieve the state object and the socket socket
            // from the asynchronous state object.
            StateObject state = (StateObject) ar.AsyncState;
            Socket socket = state.workSocket;

            // Read data from the remote device.
            int bytesRead = socket.EndReceive(ar);

            if (bytesRead > 0) {
                // There might be more data, so store the data received so far.
            state.sb.Append(Encoding.ASCII.GetString(state.buffer,0,bytesRead));

                // Get the rest of the data.
                socket.BeginReceive(state.buffer,0,StateObject.BufferSize,0,
                    new AsyncCallback(ReceiveCallback), state);
            } else {
                // All the data has arrived; put it in response.
                if (state.sb.Length > 1) {
                    response = state.sb.ToString();
                }
                // Signal that all bytes have been received.
                receiveDone.Set();
            }
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    public static void Send(Socket socket, String data) {
        // Convert the string data to byte data using ASCII encoding.
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.
        socket.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), socket);
    }

    private static void SendCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.
            Socket socket = (Socket) ar.AsyncState;

            // Complete sending the data to the remote device.
            int bytesSent = socket.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            // Signal that all bytes have been sent.
            sendDone.Set();
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }
}