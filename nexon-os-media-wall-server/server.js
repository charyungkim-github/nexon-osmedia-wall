const express = require('express')
const app = express()
const http = require('http').Server(app)
const io = require('socket.io')(http)

const port = 3333;

http.listen(port, function(){
  console.log('listening on :', port)
})

io.on('connection', function(socket){

  /* On Connect */
  console.log('connected', socket.id)
  socket.emit("socket-connected");

  /* From tracking app */
  socket.on('tracking-data', function(data) {
      console.log('received : ', data)

      // send positions to game app
      io.emit('tracking-data', data)

      // DATA KEYS :: indexData, valueData
  })

  /* On Disconnect */
  socket.on('disconnect', function() {
      console.log('disconnect', socket.id)
  })
})

