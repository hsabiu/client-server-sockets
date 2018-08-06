//AUTHOR: HABIB ADO SABIU

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.regex.Pattern;

public class Server {

    // server port 
	private static final int PORT = 500;

	// default room
	private final static String generalRoom = "general";
	private static String newClient;

    // list to store all users name
	private static List<String> names = new ArrayList<>();
	// list to store all available rooms, the default room is 'general'
	private static List<String> rooms = new ArrayList<>();
    // list to store all sent messages log
	private static List<String> messageLogs = new ArrayList<>();

    // HashMap to store all 'roomUsers' together with their OutputStream
	static HashMap<String, ArrayList<OutputStream>> roomUsers = new HashMap<String, ArrayList<OutputStream>>();
	// HashMap to store available rooms and the list of all 'roomUsersNames' in each available room
	static HashMap<String, ArrayList<String>> roomUsersNames = new HashMap<String, ArrayList<String>>();

	public static void main(String[] args) throws Exception {

		// print to the terminal
		System.out.println("Server started on 127.0.0.1 port 500");
		System.out.println("Server running...");

		// initialized list and hash maps
		rooms.add(generalRoom);
		roomUsers.put("general", new ArrayList<OutputStream>());
		roomUsersNames.put("general", new ArrayList<String>());

		// a new  handler thread that will be created when ever a new client connect the server
		try (ServerSocket listener = new ServerSocket(PORT)) {
			while (true) {
				new Handler(listener.accept()).start();
			}
		}
	}

	// handler thread implementation
	private static class Handler extends Thread {

		private String name;
		private Socket socket;
		private InputStream in;
		private OutputStream out;

		// initialize socket variables 
		public Handler(Socket socket) {
			this.socket = socket;
		}

		// run the thread
		@Override
		public void run() {

			try {

				// initialize input and output streams
				in = socket.getInputStream();
				out = socket.getOutputStream();

				while (true) {

					// the firt message to send to a client that is trying to connect is 'SUBMITNAME'
					// which is asking the client for it's user name
					sendMessage("SUBMITNAME", out);
					out.flush();

					name = receiveMessage(in);

					// if the name choosen by the client does not exist on the server, register the client with this user name
					if (name == null) {
						return;

					
					// else if the received user name already exist on the server, send 'NAMEEXIST' error to the client
					} else if (names.contains(name)) {
						sendMessage("NAMEEXIST", out);
						out.flush();
						return;
					}

					// send 'NAMEACCEPTED' message to the server
					// add the user name to list of users
					// send a 'MESSAGE' to the client indicating it has now connect to the server
					synchronized (names) {
						if (!names.contains(name)) {
							sendMessage("NAMEACCEPTED", out);

							names.add(name);
							String TimeStamp = new java.util.Date().toString();
							newClient = name + " connected on " + TimeStamp;
							sendMessage("MESSAGE " + "Connected on " + TimeStamp, out);
							out.flush();

							System.out.println(newClient);

							break;
						}
					}
				}

				// keep receiving messages from clients 
				while (true) {

					String input = receiveMessage(in);

					if (input == null) {
						return;

					// if the 'input' starts with the keyword '/users' get the list of all users from 'names' list 
                    // and send it back to the client
					} else if (input.startsWith("/users")) {

						String[] inputParts = input.split(Pattern.quote(" "));

						if (inputParts.length < 2) {
							sendMessage("MESSAGE *** All Users ***", out);
							for (int i = 0; i < names.size(); i++) {
								sendMessage("MESSAGE " + (i + 1) + ". " + names.get(i), out);
							}
							sendMessage("MESSAGE *******************", out);
							out.flush();

						} else {
							sendMessage("MESSAGE *** Users ***", out);
							for (int i = 0; i < roomUsersNames.get(inputParts[1]).size(); i++) {
								sendMessage("MESSAGE " + (i + 1) + ". " + roomUsersNames.get(inputParts[1]).get(i),
										out);
							}
							sendMessage("MESSAGE ****************", out);
							out.flush();
						}

					// else if the 'input' starts with the keyword '/rooms' get the list of all users from 'rooms' list 
                    // and send it back to the client
					} else if (input.equalsIgnoreCase("/rooms")) {
						sendMessage("MESSAGE *** Rooms ***", out);
						for (int i = 0; i < rooms.size(); i++) {
							sendMessage("MESSAGE " + (i + 1) + ". " + rooms.get(i), out);
						}
						sendMessage("MESSAGE ****************", out);
						out.flush();

					// else if the 'input' starts with the keyword '/create', create a new room and add it to 'rooms' list
					} else if (input.startsWith("/create")) {
						String[] inputParts = input.split(Pattern.quote(" "));

						if (rooms.contains(inputParts[1])) {
							sendMessage("MESSAGE server>> Room already exist", out);
							out.flush();
						} else {
							rooms.add(inputParts[1]);
							roomUsers.put(inputParts[1], new ArrayList<OutputStream>());
							roomUsersNames.put(inputParts[1], new ArrayList<String>());

							sendMessage("MESSAGE server>> " + inputParts[1] + " room created", out);
							out.flush();
						}

                    // else if 'input' starts with the keyword '/join'
                    //    if the room is not found in 'rooms' list, send 'room not found' error back to the client
					//    else, if the user is already in the room, send 'already in room' error back to the client
					//          else, send message to all client connected to this room showing a new user is connected
					//				  send success message to the client
					//			      send all the previous messages in this room to the client
					} else if (input.startsWith("/join")) {
						String[] inputParts = input.split(Pattern.quote(" "));
						if (!(rooms.contains(inputParts[1]))) {
							sendMessage("MESSAGE server>> " + inputParts[1] + " room not found", out);
							out.flush();
						} else {

							if (roomUsersNames.get(inputParts[1]).contains(name)) {
								sendMessage("MESSAGE server>> You are already in this room", out);

								out.flush();
							} else {

								for (OutputStream writer : roomUsers.get(inputParts[1])) {
									sendMessage("MESSAGE server>> " + name + " joined", writer);

									out.flush();
								}

								roomUsers.get(inputParts[1]).add(out);
								roomUsersNames.get(inputParts[1]).add(name);

								sendMessage("JOINSUCCESS server>> You have joined " + inputParts[1] + " room", out);
								out.flush();

								for (int i = 0; i < messageLogs.size(); i++) {
									if (messageLogs.get(i).startsWith(inputParts[1])) {

										String[] strMessage = messageLogs.get(i).split(Pattern.quote(" "));

										List<String> message = new ArrayList<>();

										for (String y : strMessage) {
											message.add(y);
										}

										message.remove(0);
										String name = message.get(message.size() - 1);
										message.remove(message.size() - 1);
										String strNewMessage = String.join(" ", message);
										sendMessage("MESSAGE " + name + ": " + strNewMessage, out);
										out.flush();
									}
								}

								out.flush();
							}
						}

                    // else if 'input' starts with the keyword '/join'
					//          check to make sure the command has the room name to leave
					//          if room name does not exist, send 'invalid room name' error back to the client
					//			else if user is not in the room, send 'user not in this room error back to the client'
				    //			else, send leave success message back to the client
					//			      notify all other clients connected to this room that a user has left
					} else if (input.startsWith("/leave")) {
						String[] inputParts = input.split(Pattern.quote(" "));

						if (inputParts.length < 2) {
							sendMessage("MESSAGE server>> Invalid command", out);
							out.flush();
						} else {

							if (!(rooms.contains(inputParts[1]))) {
								sendMessage("MESSAGE server>> Invalid room name", out);
								out.flush();
							} else if (!(roomUsersNames.get(inputParts[1]).contains(name))) {
								sendMessage("MESSAGE server>> You are not in this room", out);
								out.flush();
							} else {
								roomUsers.get(inputParts[1]).remove(out);
								roomUsersNames.get(inputParts[1]).remove(name);

								sendMessage("LEAVESUCCESS server>> You have leave " + inputParts[1] + " room", out);
								out.flush();

								for (OutputStream writer : roomUsers.get(inputParts[1])) {
									sendMessage("MESSAGE server>> " + name + " leave", writer);
									out.flush();
								}
							}
						}
					}

                    // else if 'input' starts with the keyword '/disconnect'
                    //          close this client output stream
					//			notify all other clients connected to this room that a user has left

					else if (input.startsWith("/disconnect")) {
						String[] inputParts = input.split(Pattern.quote(" "));

						if (inputParts.length < 2) {
							out.close();
						} else {

							roomUsers.get(inputParts[1]).remove(out);
							roomUsersNames.get(inputParts[1]).remove(name);
							out.close();

							for (OutputStream writer : roomUsers.get(inputParts[1])) {
								sendMessage("MESSAGE server>> " + name + " leave", writer);
								out.flush();
							}
						}
					}

					// else, if the user name receive from the client is 'newuser', send an error message that the client needs to connect
					//          to a group first before it can send messages
					//        else, send the received message to all clients connected this room    
					else {

						String[] inputParts = input.split(Pattern.quote(" "));

						if (inputParts[0].equalsIgnoreCase("newuser")) {
							sendMessage("MESSAGE Server>> You are not connected to any room", out);
							out.flush();

						} else {

							messageLogs.add(input + " " + name);

							for (OutputStream writer : roomUsers.get(inputParts[0])) {
								input = input.replace(inputParts[0], "");
								sendMessage("MESSAGE " + name + ": " + input, writer);
								out.flush();
							}
						}
					}
				}
			} catch (IOException e) {
				System.out.println(e);
			} catch (Exception e) {
				e.printStackTrace();
			} finally {

				if (name != null) {
					// remove this client name from list to users so other clients can be able to use this name
					names.remove(name);
				}
				if (out != null) {
					// writers.remove(out);
				}
				try {
					//close the socket
					socket.close();
				} catch (IOException e) {
				}
			}
		}
	}

    // this method is used to send messages to clients 
	public static void sendMessage(String msg, OutputStream out2) throws Exception {
		String toSend = msg;
		byte[] toSendBytes = toSend.getBytes();
		int toSendLen = toSendBytes.length;
		byte[] toSendLenBytes = new byte[4];
		toSendLenBytes[0] = (byte) (toSendLen & 0xff);
		toSendLenBytes[1] = (byte) ((toSendLen >> 8) & 0xff);
		toSendLenBytes[2] = (byte) ((toSendLen >> 16) & 0xff);
		toSendLenBytes[3] = (byte) ((toSendLen >> 24) & 0xff);
		out2.write(toSendLenBytes);
		out2.write(toSendBytes);
	}

	// this method is used to receive messages from clients
	public static String receiveMessage(InputStream in2) throws IOException {
		byte[] lenBytes = new byte[4];
		in2.read(lenBytes, 0, 4);
		int len = (((lenBytes[3] & 0xff) << 24) | ((lenBytes[2] & 0xff) << 16) | ((lenBytes[1] & 0xff) << 8)
				| (lenBytes[0] & 0xff));
		byte[] receivedBytes = new byte[len];
		in2.read(receivedBytes, 0, len);
		String returnMessage = new String(receivedBytes, 0, len);
		return returnMessage;
	}

}