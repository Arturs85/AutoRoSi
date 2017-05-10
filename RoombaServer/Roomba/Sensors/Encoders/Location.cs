using System;
using Microsoft.SPOT;

namespace RoombaServer.Roomba.Sensors.Encoders
{
  public  class Location
    {
public double HeadingDegrees { get; set; }
public double HeadingRadians { get; set; }
public double TotalDistanceFromStartPoint { get; set; }
public double X { get; set; }
public double Y { get; set; }


    }
}
