# Lab2_Networks
A project in which I built a Chat lobby in Unity using the UDP and TCP protocols.

## Problems I had
I had some issues making TCP work. In class it works properly (without the bonus, just TODOs) but when I try to debug with my machine at home it doesn't work as intended. For this reason I made only the UDP part of the activity, with all the bonuses.

## Instructions
The IP the Client connects to is hardcoded in the Client script. The built projects have my IP in the clients, for it to work in your machine you have to build it again, but the server should be good to go.

## Details
The server recieves the messages of the users and sends them all back to all the connected clients. Clients are shown in the logs as their IP and port. This could be changed to Usernames if necessary in future possible iterations.

Connecting to a server as a client adds the client to an Endpoint list in the server. Clients can see messages of all Endpoints as their IP.
