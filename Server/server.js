var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);
var port = 3000;

server.listen(port);

app.get('/', function (req, res) {
  res.sendFile(__dirname + '/index.html');
});

io.on('connection', function (socket) {
  socket.on('SocketTest', function (data) {
    console.log(data);
  });
});

io.on('connection', function (socket) {
  socket.on('chat message', function (data) {
    console.log(data);
	socket.emit('chat message', data);
  });
});

console.log(`\nServer is listening on port ${port}.\nGoto http://localhost:${port} to open the interface.`);