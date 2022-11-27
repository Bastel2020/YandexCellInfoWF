using SharpKml.Base;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YandexCellInfoWF.Models;

namespace YandexCellInfoWF.Services
{
    public static class KmlService
    {
        public static async Task<string> GetKMLAsync(IEnumerable<BaseItemInfo> input)
        {
            return await Task.Run(() =>
            {
                return GetKML(input);
            });
        }

        public static string GetKML(IEnumerable<BaseItemInfo> input)
        {
            var kml = new Kml();
            var folder = new Folder();
            foreach (var enb in input)
            {
                var point = new Point() { Coordinate = new SharpKml.Base.Vector(enb.Latitude, enb.Longitude) };
                var placemark = new Placemark
                {
                    Name = enb.Number.ToString(),
                    Geometry = point,
                    Description = new Description() { Text = $"Точность: {enb.Precision}м." },
                };
                folder.AddFeature(placemark);
            }
            kml.Feature = folder;
            Serializer serializer = new Serializer();
            serializer.Serialize(kml);
            return serializer.Xml;
        }

        public static async Task<string> GetKMLAsync(IEnumerable<EnbFullInfo> input)
        {
            return await Task.Run(() =>
            {
                return GetKML(input);
            });
        }

        public static string GetKML(IEnumerable<EnbFullInfo> input)
        {
            var kml = new Kml();
            var folder = new Folder();

            foreach (var enb in input)
            {
                var enbFolder = new Folder();
                enbFolder.Name = enb.Enb.ToString();

                var point = new Point() { Coordinate = new SharpKml.Base.Vector(enb.Latitude, enb.Longitude) };
                var placemark = new Placemark
                {
                    Name = enb.Enb.ToString(),
                    Geometry = point
                };

                if (enb.Sectors.Count > 1)
                    foreach (var sector in enb.Sectors)
                    {
                        var c = new CoordinateCollection();
                        c.Add(new Vector(sector.Latitude, sector.Longitude));
                        c.Add(new Vector(enb.Latitude, enb.Longitude));
                        var lineString = new LineString() { Coordinates = c };
                        var linePlacemark = new Placemark
                        {
                            Name = $"{enb.Enb} : {sector.Number}",
                            Geometry = lineString,
                            Visibility = true
                        };

                        var lineStyle = new LineStyle();
                        lineStyle.Color = new Color32(255, 0, 255, 255);
                        lineStyle.Width = 3;
                        var x = new Style();
                        x.Line = lineStyle;
                        linePlacemark.AddStyle(x);

                        enbFolder.AddFeature(linePlacemark);
                    }
                else if (enb.Sectors.Count != 0)
                    placemark.Name = enb.Enb.ToString() + " : " + enb.Sectors[0].Number;
                enbFolder.AddFeature(placemark);
                folder.AddFeature(enbFolder);
            }
            kml.Feature = folder;

            Serializer serializer = new Serializer();
            serializer.Serialize(kml);
            return serializer.Xml;
        }
    }
}
