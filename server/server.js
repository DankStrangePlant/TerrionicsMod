var port = process.env.PORT || 3005;

var net = require('net');

var clients = [];

net.createServer(function (socket) {

  console.log("CONNECTED");

  socket.on('data', function (data) {
    console.log(data.toString());
  });

  socket.on('end', function () {
    console.log("DISCONNECTED");
  });

}).listen(port);

console.log("Running on,", port);
