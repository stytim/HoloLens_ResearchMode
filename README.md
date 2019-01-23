# HoloLens_ReseachMode

This project is to exploit the Microsoft HoloLens’s raw sensor data.

The research mode allows us to access key HoloLens sensors including: 
* The four environment tracking cameras 
* Two versions of the depth mapping camera data  
I. Short-range depth camera can sense depth from about 0.15m to 0.95m;  
II. Long-range depth camera can sense depth from about 0.95m to 3.52m. 
* Two versions of an IR-reflectivity stream 

## Enabling Research Mode: 
Follow this link to enable research mode to access sensor data:
https://docs.microsoft.com/en-us/windows/mixed-reality/research-mode 

## Settings that We Need to Change in a C# Project: 
Since the use of sensor data is a Microsoft restricted capability, we need to change some of the setting in Package.appxmanifest when building c# project.  

```
<Package
  xmlns="http://schemas.microsoft.com/appx/manifest/foundation/windows10"
  xmlns:mp="http://schemas.microsoft.com/appx/2014/phone/manifest"
  xmlns:uap="http://schemas.microsoft.com/appx/manifest/uap/windows10"
  xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities"
  IgnorableNamespaces="uap mp rescap"
  >
...
  <Capabilities>
    <rescap:Capability Name="perceptionSensorsExperimental"/>
    <DeviceCapability Name="webcam"/>
  </Capabilities>
</Package>
```
