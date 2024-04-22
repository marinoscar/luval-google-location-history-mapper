using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luval.glhp.Nodes
{
    public class SourceInfo
    {
        [JsonProperty("deviceTag")]
        public string DeviceTag { get; set; }
    }

    public class Location
    {
        [JsonProperty("latitudeE7")]
        public double LatitudeE7 { get; set; }

        [JsonProperty("longitudeE7")]
        public double LongitudeE7 { get; set; }

        [JsonProperty("placeId")]
        public string PlaceId { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("semanticType")]
        public string SemanticType { get; set; }

        [JsonProperty("sourceInfo")]
        public SourceInfo SourceInfo { get; set; }

        [JsonProperty("locationConfidence")]
        public double LocationConfidence { get; set; }

        [JsonProperty("calibratedProbability")]
        public double CalibratedProbability { get; set; }
    }

    public class Duration
    {
        [JsonProperty("startTimestamp")]
        public DateTime? StartTimestamp { get; set; }

        [JsonProperty("endTimestamp")]
        public DateTime? EndTimestamp { get; set; }
    }

    public class Activity
    {
        [JsonProperty("activityType")]
        public string ActivityType { get; set; }

        [JsonProperty("probability")]
        public double Probability { get; set; }
    }

    public class Waypoint
    {
        [JsonProperty("latE7")]
        public long LatE7 { get; set; }

        [JsonProperty("lngE7")]
        public long LngE7 { get; set; }
    }

    public class RoadSegment
    {
        [JsonProperty("placeId")]
        public string PlaceId { get; set; }

        [JsonProperty("duration")]
        public string Duration { get; set; }
    }

    public class WaypointPath
    {
        [JsonProperty("waypoints")]
        public List<Waypoint> Waypoints { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("roadSegment")]
        public List<RoadSegment> RoadSegments { get; set; }

        [JsonProperty("distanceMeters")]
        public double DistanceMeters { get; set; }

        [JsonProperty("travelMode")]
        public string TravelMode { get; set; }

        [JsonProperty("confidence")]
        public double Confidence { get; set; }
    }

    public class SimplifiedRawPathPoint
    {
        [JsonProperty("latE7")]
        public long LatE7 { get; set; }

        [JsonProperty("lngE7")]
        public long LngE7 { get; set; }

        [JsonProperty("accuracyMeters")]
        public int AccuracyMeters { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public class SimplifiedRawPath
    {
        [JsonProperty("points")]
        public List<SimplifiedRawPathPoint> Points { get; set; }
    }

    public class ChildVisit
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("placeConfidence")]
        public string PlaceConfidence { get; set; }

        [JsonProperty("visitConfidence")]
        public int? VisitConfidence { get; set; }

        [JsonProperty("otherCandidateLocations")]
        public List<CandidateLocation> OtherCandidateLocations { get; set; }

        [JsonProperty("editConfirmationStatus")]
        public string EditConfirmationStatus { get; set; }

        [JsonProperty("locationConfidence")]
        public double? LocationConfidence { get; set; }

        [JsonProperty("placeVisitType")]
        public string PlaceVisitType { get; set; }

        public override string ToString()
        {
            return ToCsv();
        }

        public string ToCsv()
        {
            return $"@account,{Location?.LatitudeE7},{Location?.LongitudeE7},\"{Location?.Address}\",{Location?.LocationConfidence},{Location?.SourceInfo?.DeviceTag},{Duration?.StartTimestamp},{Duration?.EndTimestamp},{PlaceConfidence},{PlaceVisitType}";
        }

        public static string CsvHeader()
        {
            return "Account,LatitudeE7,LongitudeE7,Address,LocationConfidence,DeviceTag,StartTimestamp,EndTimestamp,PlaceConfidence,PlaceVisitType";
        }
    }

    public class CandidateLocation
    {
        [JsonProperty("latitudeE7")]
        public int? LatitudeE7 { get; set; }

        [JsonProperty("longitudeE7")]
        public int? LongitudeE7 { get; set; }

        [JsonProperty("placeId")]
        public string PlaceId { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("semanticType")]
        public string SemanticType { get; set; }

        [JsonProperty("locationConfidence")]
        public double? LocationConfidence { get; set; }

        [JsonProperty("calibratedProbability")]
        public double? CalibratedProbability { get; set; }
    }

    public class ActivitySegment : INode
    {
        [JsonProperty("startLocation")]
        public Location StartLocation { get; set; }

        [JsonProperty("endLocation")]
        public Location EndLocation { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("activityType")]
        public string ActivityType { get; set; }

        [JsonProperty("confidence")]
        public string Confidence { get; set; }

        [JsonProperty("activities")]
        public List<Activity> Activities { get; set; }

        [JsonProperty("waypointPath")]
        public WaypointPath WaypointPath { get; set; }

        [JsonProperty("simplifiedRawPath")]
        public SimplifiedRawPath SimplifiedRawPath { get; set; }

        public ActivitySegment CastAsActivitySegment()
        {
            return this;
        }

        public PlaceVisit CastAsPlaceVisit()
        {
            throw new InvalidOperationException();
        }

        public bool IsActivitySegment()
        {
            return true;
        }

        public bool IsPlaceVisit()
        {
            return false;
        }
    }

    public class PlaceVisit : INode
    {
        public Location Location { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("placeConfidence")]
        public string PlaceConfidence { get; set; }

        [JsonProperty("childVisits")]
        public List<ChildVisit> ChildVisits { get; set; }

        [JsonProperty("visitConfidence")]
        public int? VisitConfidence { get; set; }

        [JsonProperty("otherCandidateLocations")]
        public List<CandidateLocation> OtherCandidateLocations { get; set; }

        [JsonProperty("editConfirmationStatus")]
        public string EditConfirmationStatus { get; set; }

        [JsonProperty("locationConfidence")]
        public double? LocationConfidence { get; set; }

        [JsonProperty("placeVisitType")]
        public string PlaceVisitType { get; set; }

        [JsonProperty("placeVisitImportance")]
        public string PlaceVisitImportance { get; set; }

        public ActivitySegment CastAsActivitySegment()
        {
            throw new InvalidOperationException();
        }

        public PlaceVisit CastAsPlaceVisit()
        {
            return this;
        }

        public bool IsActivitySegment()
        {
            return false;
        }

        public bool IsPlaceVisit()
        {
            return true;
        }

        public override string ToString()
        {
            return ToCsv();
        }

        public string ToCsv()
        {
            return string.Join(Environment.NewLine, GetVisits().Select(i => i.ToCsv()));
        }

        public string CsvHeader()
        {
            return ChildVisit.CsvHeader();
        }

        public List<ChildVisit> GetVisits()
        {
            var result = new List<ChildVisit>();
            result.Add(ConvertToVisit());
            if(this.ChildVisits != null && this.ChildVisits.Any())
                result.AddRange(this.ChildVisits);
            return result;
        }

        public ChildVisit ConvertToVisit()
        {
            return new ChildVisit()
            {
                Duration = this.Duration,
                EditConfirmationStatus = this.EditConfirmationStatus,
                Location = this.Location,
                LocationConfidence = this.LocationConfidence,
                OtherCandidateLocations = this.OtherCandidateLocations,
                PlaceVisitType = this.PlaceVisitType,
                PlaceConfidence = this.PlaceConfidence,
                VisitConfidence = this.VisitConfidence,
            };
        }
    }

    public interface INode
    {
        bool IsPlaceVisit();
        bool IsActivitySegment();

        PlaceVisit CastAsPlaceVisit();
        ActivitySegment CastAsActivitySegment();

    }

    public class LocationHistory
    {
        [JsonProperty("timelineObjects")]
        public List<JObject> TimelineObjects { get; set; }

        public List<PlaceVisit> GetPlaceVisits()
        {
            var result = new List<PlaceVisit>();
            foreach (var item in TimelineObjects)
            {
                if (item["placeVisit"] != null)
                {
                    result.Add(item["placeVisit"].ToObject<PlaceVisit>());
                }
            }
            return result;
        }
    }
}
