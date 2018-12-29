import * as usb from 'usb';

let getRowers = () => usb.getDeviceList().filter(device => device.deviceDescriptor.idVendor.toString(16) == '17a4')

export default getRowers;