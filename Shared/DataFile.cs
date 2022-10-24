namespace SampleApp.Shared
{
    public class DataFile
    {
        public DataFile()
        {

        }
        
        public DataFile(Guid id)
        {
            this.Id = id;
        }

        public string FileName => $"file_{this.Id}.csv";
        
        public string FilePath => $"raw/{this.FileName}";
        
        public string Url { get; set; }
        
        public Guid Id { get; set; }
    }
}
