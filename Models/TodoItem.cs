namespace TodoApi.Models{
    public class TodoItem{
        //what is written here becomes the json object
        public long Id {get; set;}
        public string Name {get; set;}
        public bool IsComplete {get; set;}
    }
}
