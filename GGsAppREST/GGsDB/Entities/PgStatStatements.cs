namespace GGsDB.Entities
{
    public partial class PgStatStatements
    {
        public uint? Userid { get; set; }
        public uint? Dbid { get; set; }
        public long? Queryid { get; set; }
        public string Query { get; set; }
        public long? Calls { get; set; }
        public double? TotalTime { get; set; }
        public double? MinTime { get; set; }
        public double? MaxTime { get; set; }
        public double? MeanTime { get; set; }
        public double? StddevTime { get; set; }
        public long? Rows { get; set; }
        public long? SharedBlksHit { get; set; }
        public long? SharedBlksRead { get; set; }
        public long? SharedBlksDirtied { get; set; }
        public long? SharedBlksWritten { get; set; }
        public long? LocalBlksHit { get; set; }
        public long? LocalBlksRead { get; set; }
        public long? LocalBlksDirtied { get; set; }
        public long? LocalBlksWritten { get; set; }
        public long? TempBlksRead { get; set; }
        public long? TempBlksWritten { get; set; }
        public double? BlkReadTime { get; set; }
        public double? BlkWriteTime { get; set; }
    }
}
