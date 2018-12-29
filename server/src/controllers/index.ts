import { Response, Request, Router} from  'express';
import * as usb from 'usb';

const router =  Router();

router.get('/', (req: Request, res: Response) => {
  res.json({title: 'Express'});
});

router.get('/devices', (req: Request, res: Response) => {
  var dl = usb.getDeviceList();
  let rower = new usb.Device();

  for(var i = 0; i < dl.length; i++){
    // if(parseInt(dl[i].deviceDescriptor.idVendor).toString(16)=='17a4'){
    if(dl[i].deviceDescriptor.idVendor.toString(16)=='17a4'){
      rower = dl[i];
    }
  }

  rower.open();

  //var rowerInt = rower.interfaces[0];

  var c2r = rower.interfaces[0].endpoints[1];

  var requestType = usb.LIBUSB_REQUEST_TYPE_STANDARD;

  rower.controlTransfer(0x21,0x09, 0x0, 0x0, new Buffer([0xF0, 0xFD, 0x00, 0x80, 0x80, 0xF2]),
    function(error, data){
      console.log(error);
      console.log(data);
    }
  );
});

export default router;