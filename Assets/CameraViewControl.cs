using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_UWP

using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Capture.Frames;
using Windows.Media.Capture;
using Windows.Media.Devices.Core;
using System.Runtime.InteropServices.WindowsRuntime;

#endif

public class CameraViewControl : MonoBehaviour
{

    private Texture2D tex = null;
    private byte[] bytes = null;
    public Text output;
    public Text depthmeter;
  //  private float tempdepth;
    private int test;
    private float maxdepth;
    private float mindepth;
    // Use this for initialization
    void Start()
    {
        test = 0;
#if UNITY_UWP
        Task.Run(() => { InitSensor(); });
#endif
    }

    // Update is called once per frame
    void Update()
    {
        switch (test)
        {
            case 0:

                output.text = "Not updated!";
                break;

            case 1:

                output.text = "Color!";

                break;
            case 2:

                // output.text = "Depth!";
                // output.text = " ";
                
                depthmeter.text = "W:" + maxdepth + " H:" + mindepth;
                //depthmeter.text = " Jesus";
                break;
            case 3:

                output.text = "None!";
                break;

        }

    }

#if UNITY_UWP

    private async void InitSensor()
    {
        var mediaFrameSourceGroupList = await MediaFrameSourceGroup.FindAllAsync();
        var mediaFrameSourceGroup = mediaFrameSourceGroupList[1];
        var mediaFrameSourceInfo = mediaFrameSourceGroup.SourceInfos[0];
        var mediaCapture = new MediaCapture();
        var settings = new MediaCaptureInitializationSettings()
        {
            SourceGroup = mediaFrameSourceGroup,
            SharingMode = MediaCaptureSharingMode.SharedReadOnly,
            StreamingCaptureMode = StreamingCaptureMode.Video,
            MemoryPreference = MediaCaptureMemoryPreference.Cpu,
        };
        try
        {
            await mediaCapture.InitializeAsync(settings);
            test = 2;
            var mediaFrameSource = mediaCapture.FrameSources[mediaFrameSourceInfo.Id];
           // var cameraIntrisics = mediaFrameSource.TryGetCameraIntrinsics(mediaFrameSource.CurrentFormat);
           // maxdepth = cameraIntrisics.FocalLength.X;
           // maxdepth = cameraIntrisics.FocalLength.Y;
            var mediaframereader = await mediaCapture.CreateFrameReaderAsync(mediaFrameSource, mediaFrameSource.CurrentFormat.Subtype);  
            mediaframereader.FrameArrived += FrameArrived;
            await mediaframereader.StartAsync();
            
        }
        catch (Exception e)
        {
            UnityEngine.WSA.Application.InvokeOnAppThread(() => { Debug.Log(e); }, true);
        }
    }

    private void FrameArrived(MediaFrameReader sender, MediaFrameArrivedEventArgs args)
    {
        var mediaframereference = sender.TryAcquireLatestFrame();
        if (mediaframereference != null)
        {
            var videomediaframe = mediaframereference?.VideoMediaFrame;
            
            CameraIntrinsics camerI = videomediaframe?.CameraIntrinsics;
            if (camerI != null)
            {
                maxdepth = camerI.ImageHeight;
                mindepth = camerI.ImageWidth;
               // mindepth = 4.5f;
            }
          
            var softwarebitmap = videomediaframe?.SoftwareBitmap;
            if (softwarebitmap != null)
            {
  
              //  softwarebitmap = SDKTemplate.FrameRenderer.ConvertToDisplayableImage(videomediaframe);
                          
                softwarebitmap = SoftwareBitmap.Convert(softwarebitmap, BitmapPixelFormat.Rgba8, BitmapAlphaMode.Premultiplied);
                int w = softwarebitmap.PixelWidth;
                int h = softwarebitmap.PixelHeight;
              //  maxdepth = w;
              //  mindepth = h;
                if (bytes == null)
                {
                    bytes = new byte[w * h * 4];
                }
                softwarebitmap.CopyToBuffer(bytes.AsBuffer());
                softwarebitmap.Dispose();
                UnityEngine.WSA.Application.InvokeOnAppThread(() => {
                    if (tex == null)
                    {
                        tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
                        GetComponent<Renderer>().material.mainTexture = tex;
                    }
                    for (int i = 0; i < bytes.Length / 4; ++i)
                    {
                       bytes[i * 4 + 3] = 255;
                    }
                    
                    tex.LoadRawTextureData(bytes);
                    tex.Apply();
                }, true);
            }
            mediaframereference.Dispose();
        }
    }

#endif
}
