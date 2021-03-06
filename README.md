# How Do I Sample for Android

### Description

The “How Do I?” samples collection is a comprehensive set containing dozens of interactive samples. Available in C#, these samples are designed to hit all the highlights of Map Suite, from simply adding a layer to a map to performing spatial queries and applying a thematic style. Consider this collection your “encyclopedia” of all the Map Suite basics and a great starting place for new users.

Please refer to [Wiki](http://wiki.thinkgeo.com/wiki/map_suite_mobile_for_android) for the details.

![Screenshot](https://github.com/ThinkGeo/HowDoISample-ForAndroid/blob/master/ScreenShot.png)

### Requirements
This sample makes use of the following NuGet Packages

[MapSuite 10.0.0](https://www.nuget.org/packages?q=ThinkGeo)

### About the Code

```csharp
WorldMapKitOverlay layerOverlay = new WorldMapKitOverlay();

androidMap = FindViewById<MapView>(Resource.Id.androidmap);
androidMap.MapUnit = GeographyUnit.DecimalDegree;
androidMap.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
androidMap.Overlays.Add(layerOverlay);
```

### Getting Help

- [Map Suite mobile for Android Wiki Resources](http://wiki.thinkgeo.com/wiki/map_suite_mobile_for_android)
- [Map Suite mobile for Android Product Description](https://thinkgeo.com/ui-controls#mobile-platforms)
- [ThinkGeo Community Site](http://community.thinkgeo.com/)
- [ThinkGeo Web Site](http://www.thinkgeo.com)

### Key APIs
This example makes use of the following APIs:

- [ThinkGeo.MapSuite.Android.MapView](http://wiki.thinkgeo.com/wiki/api/thinkgeo.mapsuite.android.mapview)
- [ThinkGeo.MapSuite.Android.LayerOverlay](http://wiki.thinkgeo.com/wiki/api/thinkgeo.mapsuite.android.layeroverlay)
- [ThinkGeo.MapSuite.Shapes.RectangleShape](http://wiki.thinkgeo.com/wiki/api/thinkgeo.mapsuite.shapes.rectangleshape)

### About Map Suite
Map Suite is a set of powerful development components and services for the .Net Framework.

### About ThinkGeo
ThinkGeo is a GIS (Geographic Information Systems) company founded in 2004 and located in Frisco, TX. Our clients are in more than 40 industries including agriculture, energy, transportation, government, engineering, software development, and defense.
