var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);
var port = 5000;

server.listen(port);

app.get('/', function (req, res) {
  res.sendFile(__dirname + '/index.html');
});

io.on('connection', function (socket) {
  console.log("connected");
  socket.on('SocketTest', function (data) {
    console.log(data);
  });
  socket.on('disconnect', function(){
        console.log('disconnected');
    });
  socket.on('socket-connected', function (data) {
    io.sockets.emit('news', data);
  });
  
  socket.on('chat message', function (data) {
	if(data.substring(0, "Spawn ".length) == "Spawn ")
		io.sockets.emit('spawn item', data.substring("Spawn ".length, data.length));
	else
		io.sockets.emit('chat message', data);
  });
  socket.on('player-position', function (data) {
	  io.sockets.emit('display-player-position', data);
	  
  });
});

console.log(`\nServer is listening on port ${port}.\nGoto http://localhost:${port} to open the interface.`);