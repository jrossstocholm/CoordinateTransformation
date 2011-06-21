using System;
using Ciber.Core.Coordinates.VB;
using NUnit.Framework;

namespace Coordinates.Test
{
    [TestFixture]
    public class TestOfVBTransformation
    {
        private decimal _delta;

        [TestFixtureSetUp]
        public void DoSetup()
        {
            _delta = 0.00001m;
        }

        [Test]
        public void TestSimpleTransform()
        {
            const decimal latitude = 51.6702014m;
            const decimal longitude = -76.1103928m;

            var latCoord = new CoordTrans.Coordinate(latitude, CoordTrans.CoordinateType.Latitude);
            var lngCoord = new CoordTrans.Coordinate(longitude, CoordTrans.CoordinateType.Longitude);

            var utm = new CoordTrans.UTM(latCoord, lngCoord);
        }

        [Test]
        public void CanConvertFromEtrs86ToWgs84_Latitude()
        {
            //Arrange
            const double etrs85N = 6227738.595;
            const double etrs85E = 594538.991;

            //Act
            var coordinate = new CoordTrans.UTM(etrs85N, etrs85E, 32);

            //Assert
            Assert.AreEqual(@"56° 11' 6,779"" North", coordinate.Latitude.ToString());
            Assert.LessOrEqual(Math.Abs(coordinate.Latitude.Radian - 56.1852203615381m), _delta);
            Assert.AreEqual(coordinate.Datum, CoordTrans.UTMDatum.WGS84);
            Assert.AreEqual(coordinate.Easting, etrs85E);
            Assert.AreEqual(coordinate.Northing, etrs85N);
            Assert.AreEqual(coordinate.UTMLatitudeHemisphere, CoordTrans.UTMLat.North);
        }

        [Test]
        public void CanConvertFromEtrs86ToWgs84_Longitude()
        {
            //Arrange
            const double etrs85N = 6227738.595;
            const double etrs85E = 594538.991;

            //Act
            var coordinate = new CoordTrans.UTM(etrs85N, etrs85E, 32);

            //Assert
            var longitude = coordinate.Longitude.ToString();
            Assert.AreEqual(@"10° 31' 23,486"" East", longitude);
            Assert.LessOrEqual(Math.Abs(coordinate.Longitude.Radian - 10.5231905076506m), _delta);
            Assert.AreEqual(coordinate.Datum, CoordTrans.UTMDatum.WGS84);
            Assert.AreEqual(coordinate.Easting, etrs85E);
            Assert.AreEqual(coordinate.Northing, etrs85N);
            Assert.AreEqual(coordinate.UTMLatitudeHemisphere, CoordTrans.UTMLat.North);
        }

        #region Parameterized tests

        [TestCase(6227738.595, 594538.991, CoordTrans.UTMLat.North, 32, Result = CoordTrans.UTMLat.North)]
        [TestCase(7020000, 300000, CoordTrans.UTMLat.North, 14, Result = CoordTrans.UTMLat.North)]
        [TestCase(4041000, 310000, CoordTrans.UTMLat.South, 45, Result = CoordTrans.UTMLat.South)]
        [TestCase(1000000, 320000, CoordTrans.UTMLat.North, 30, Result = CoordTrans.UTMLat.North)]
        [TestCase(2000000, 330000, CoordTrans.UTMLat.South, 2, Result = CoordTrans.UTMLat.South)]
        [TestCase(3000000, 340000, CoordTrans.UTMLat.North, 22, Result = CoordTrans.UTMLat.North)]
        [TestCase(4000000, 350000, CoordTrans.UTMLat.South, 43, Result = CoordTrans.UTMLat.South)]
        [TestCase(5000000, 360000, CoordTrans.UTMLat.North, 32, Result = CoordTrans.UTMLat.North)]
        [TestCase(6000000, 370000, CoordTrans.UTMLat.South, 51, Result = CoordTrans.UTMLat.South)]
        [TestCase(7000000, 380000, CoordTrans.UTMLat.North, 23, Result = CoordTrans.UTMLat.North)]
        [TestCase(8000000, 390000, CoordTrans.UTMLat.South, 27, Result = CoordTrans.UTMLat.South)]
        [TestCase(1000000, 400000, CoordTrans.UTMLat.North, 18, Result = CoordTrans.UTMLat.North)]
        [TestCase(2000000, 410000, CoordTrans.UTMLat.South, 52, Result = CoordTrans.UTMLat.South)]
        [TestCase(3000000, 420000, CoordTrans.UTMLat.North, 37, Result = CoordTrans.UTMLat.North)]
        [TestCase(3000000, 430000, CoordTrans.UTMLat.South, 28, Result = CoordTrans.UTMLat.South)]
        [TestCase(4000000, 440000, CoordTrans.UTMLat.North, 19, Result = CoordTrans.UTMLat.North)]
        [TestCase(5002000, 450000, CoordTrans.UTMLat.South, 16, Result = CoordTrans.UTMLat.South)]
        public CoordTrans.UTMLat TestResultingHemisphere(double northing, double easting, CoordTrans.UTMLat hemisphere, int zone)
        {
            //Arrange
            
            //Act
            var coordinate = new CoordTrans.UTM(northing, easting, zone)
                                 {
                                     UTMLatitudeHemisphere = hemisphere
                                 };

            //Assert
            return coordinate.UTMLatitudeHemisphere;
        }

        //[TestCase(4041000, 310000, CoordTrans.UTMLat.South, 45, Result = @"84° 52' 43,216"" East")]
        //[TestCase(4041000, 310000, CoordTrans.UTMLat.South, 45, Result = @"84° 52' 43,216"" East")]
        //[TestCase(2000000, 330000, CoordTrans.UTMLat.South, 2,  Result = @"172° 36' 23,141"" East")]
        //[TestCase(4000000, 350000, CoordTrans.UTMLat.South, 43, Result = @"73° 19' 58,654"" East")]
        //[TestCase(6000000, 370000, CoordTrans.UTMLat.South, 51, Result = @"121° 0' 37,292"" East")]
        //[TestCase(8000000, 390000, CoordTrans.UTMLat.South, 27, Result = @"24° 12' 11,484"" East")]
        //[TestCase(2000000, 410000, CoordTrans.UTMLat.South, 52, Result = @"128° 8' 58,019"" East")]
        //[TestCase(3000000, 430000, CoordTrans.UTMLat.South, 28, Result = @"15° 42' 22,616"" East")]
        //[TestCase(5002000, 450000, CoordTrans.UTMLat.South, 16, Result = @"87° 38' 10,593"" East")]

        [TestCase(3000000,     420000,     CoordTrans.UTMLat.North, 37, Result = @"38° 11' 34,180"" East")]
        [TestCase(3000000,     340000,     CoordTrans.UTMLat.North, 22, Result = @"52° 36' 50,940"" West")]
        [TestCase(5000000,     360000,     CoordTrans.UTMLat.North, 32, Result = @"7° 13' 9,714"" East")]
        [TestCase(7020000,     300000,     CoordTrans.UTMLat.North, 14, Result = @"102° 59' 5,634"" West")]
        [TestCase(6227738.595, 594538.991, CoordTrans.UTMLat.North, 32, Result = @"10° 31' 23,486"" East")]
        [TestCase(7020000,     300000,     CoordTrans.UTMLat.North, 14, Result = @"102° 59' 5,634"" West")]
        [TestCase(1000000,     320000,     CoordTrans.UTMLat.North, 30, Result = @"4° 38' 15,447"" West")]
        [TestCase(7000000,     380000,     CoordTrans.UTMLat.North, 23, Result = @"47° 22' 42,195"" West")]
        [TestCase(1000000,     400000,     CoordTrans.UTMLat.North, 18, Result = @"75° 54' 35,566"" West")]
        [TestCase(4000000,     440000,     CoordTrans.UTMLat.North, 19, Result = @"69° 40' 0,923"" West")]
        public string TestResultingCordinateStringLongitude(double northing, double easting, CoordTrans.UTMLat hemisphere, int zone)
        {
            //Arrange

            //Act
            var coordinate = new CoordTrans.UTM(northing, easting, zone)
            {
                UTMLatitudeHemisphere = hemisphere
            };
                
            //Assert
            return coordinate.Longitude.ToString();
        }

        #endregion
    }
}
