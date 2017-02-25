var port = process.env.PORT || 3005;

var net = require('net');

var clients = [];

net.createServer(function (socket) {

  console.log("CONNECTED");

  socket.on('data', function (data)
  {
	var string = data.toString();
	var object = JSON.parse(data);
	if(object.text == "ping")
	{
	  console.log("Responding to user ping");
	  socket.write('{"type":"misc", "text":"ping"}');
	  console.log('{"type":"misc", "text":"ping"}');
	  
	}
	else
	{
      console.log(data.toString());
	}
  });

  socket.on('end', function () {
    console.log("DISCONNECTED");
  });

}).listen(port);

const dgram = require('dgram');
const udpserver = dgram.createSocket('udp4');

udpserver.on('error', (err) => {
  console.log(`server error:\n${err.stack}`);
  udpserver.close();
});

udpserver.on('message', (msg, rinfo) => {
  var string = msg.toString();
  var object = JSON.parse(msg);
  if(object.type == "position")
  {
	console.log("Player position: " + object.data);
  }
  else
  {
	console.log(`server got: ${msg} from ${rinfo.address}:${rinfo.port}`);
  }

});

udpserver.on('listening', () => {
  var address = udpserver.address();
  console.log(`server listening ${address.address}:${address.port}`);
});
udpserver.bind(5000);

console.log("Running on,", port);