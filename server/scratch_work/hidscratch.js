const HID = require('node-hid');
const EventEmitter = require('events');
const Frame = require('csafe').Frame;
const Response = require('csafe').Response;
const FrameReader = require('csafe').FrameReader;
const Command = require('csafe').Command;

this.frameReader = new FrameReader();
this.frameCount = 0;

var device = HID.devices().filter((device) => device.vendorId == 6052)[0];

var hidDevice = new HID.HID(device.path);

frameReader.on('frame', (frame) => {
  console.log('frame received');
  console.log(frame);
  console.log(`data is: `);
  console.log(frame.data);
});

hidDevice.on('data', (record) => {
  console.log('data received');
  console.log(record);
  let buffer = record.slice(1)
  
  this.frameReader.read(buffer)
});

//write stuff
const getCadenceCmd = new Command('GetCadence');
var buffer = Buffer.alloc(22);

buffer.writeInt8(1);
getCadenceCmd.buffer.copy(buffer, 1);
hidDevice.write(Array.from(buffer));

const GetTWork = new Command('GetTWork');
var buffer2 = Buffer.alloc(22);

buffer2.writeInt8(1);
GetTWork.buffer.copy(buffer2, 1);
hidDevice.write(Array.from(buffer2));

var workoutDuration = Buffer.alloc(3);
workoutDuration[0] = 0x01;
workoutDuration[1] = 0x01;
workoutDuration[2] = 0x01;

const SetWork = new Command('SetWork', workoutDuration);
var buffer3 = Buffer.alloc(22);

buffer3.writeInt8(1);
SetWork.buffer.copy(buffer3, 1);
buffer3
// hidDevice.write(Array.from(buffer3));

var manualBuffer = Buffer.from([0xF1, 0x20, 0x03, 0x00, 0x07, 0x1E, 0x3A, 0xF2]);