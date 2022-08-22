using System.Text.Json;

namespace MeetingManager
{
    // Meeting class and its properties
    public class Meeting
    {
        public string? Name { get; set; }
        public string? ResponsiblePerson { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Type { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public List<string?>? PersonList { get; set; }
        public Meeting()
        {
            this.PersonList = new List<string?>();
        }
    }

    public class Program
    {
        // deserialize JSON to a list of Meeting objects
        public static List<Meeting> JsonLoad(string fileName)
        {
            string jsonStringRead = File.ReadAllText(fileName);
            var LoadedList = JsonSerializer.Deserialize<List<Meeting>>(jsonStringRead)!;
            return LoadedList;
        }
        // serialize a list of Meeting objects to JSON
        public static void JsonWrite(string fileName, List<Meeting> meetings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(meetings, options);
            File.WriteAllText(fileName, jsonString);
        }
        public static void Main()
        {
            // initialize loop to keep console app running
            while (true)
            {
                Console.WriteLine("Meeting Manager App\r");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~\n");

                Console.WriteLine("Choose an option from the following list:");
                Console.WriteLine("\ta - Create a new meeting");
                Console.WriteLine("\tb - Delete a meeting");
                Console.WriteLine("\tc - Add a person to the meeting");
                Console.WriteLine("\td - Remove a person from the meeting");
                Console.WriteLine("\te - List all meetings");
                Console.Write("Select an option: \n");

                string fileName = "MeetingList.json";
                var options = new JsonSerializerOptions { WriteIndented = true };

                // branch out depending on the selection
                switch (Console.ReadLine())
                {
                    // create a new meeting
                    case "a":
                        Console.WriteLine($"Provide a name for the meeting:");
                        var meeting = new Meeting
                        {
                            Name = Convert.ToString(Console.ReadLine()),
                        };

                        Console.WriteLine($"Provide the name of the person responsible for the meeting:");
                        meeting.ResponsiblePerson = Convert.ToString(Console.ReadLine());
                        meeting.PersonList.Add(meeting.ResponsiblePerson);

                        Console.WriteLine($"Provide a description of the meeting:");
                        meeting.Description = Convert.ToString(Console.ReadLine());

                        Console.WriteLine($"Select the category of the meeting:");
                        Console.WriteLine("\ta - CodeMonkey");
                        Console.WriteLine("\tb - Hub");
                        Console.WriteLine("\tc - Short");
                        Console.WriteLine("\td - TeamBuilding");
                        switch (Console.ReadLine())
                        {
                            case "a":
                                meeting.Category = "CodeMonkey";
                                break;
                            case "b":
                                meeting.Category = "Hub";
                                break;
                            case "c":
                                meeting.Category = "Short";
                                break;
                            case "d":
                                meeting.Category = "TeamBuilding";
                                break;
                        }

                        Console.WriteLine($"Select the type of the meeting:");
                        Console.WriteLine("\ta - Live");
                        Console.WriteLine("\tb - InPerson");
                        switch (Console.ReadLine())
                        {
                            case "a":
                                meeting.Type = "Live";
                                break;
                            case "b":
                                meeting.Type = "InPerson";
                                break;
                        }

                        Console.WriteLine($"Provide a start date for the meeting:");
                        meeting.StartDate = Convert.ToDateTime(Console.ReadLine());

                        Console.WriteLine($"Provide an end date for the meeting:");
                        meeting.EndDate = Convert.ToDateTime(Console.ReadLine());
                        if (File.Exists(fileName))
                        {
                            // serializing meeting to JSON if file is not empty
                            if (new FileInfo(fileName).Length > 6)
                            {
                                var meetingsLoaded = JsonLoad(fileName);
                                meetingsLoaded.Add(meeting);
                                JsonWrite(fileName, meetingsLoaded);

                                Console.WriteLine("Meeting successfully added.");
                            }
                            // serializing meeting to JSON if file is empty
                            else
                            {
                                List<Meeting> meetingsWrite = new List<Meeting>();
                                meetingsWrite.Add(meeting);

                                string jsonStringWrite = JsonSerializer.Serialize(meetingsWrite, options);
                                File.AppendAllText(fileName, jsonStringWrite);

                                Console.WriteLine("Meeting successfully added.");
                            }
                        }
                        else
                        // serializing meeting to JSON if file does not exist
                        {
                            List<Meeting> meetingsWrite = new List<Meeting>();
                            meetingsWrite.Add(meeting);

                            string jsonStringWrite = JsonSerializer.Serialize(meetingsWrite, options);
                            File.AppendAllText(fileName, jsonStringWrite);

                            Console.WriteLine("Meeting successfully added.");
                        }
                        Console.WriteLine(File.ReadAllText(fileName));
                        break;
                    // delete a meeting
                    case "b":
                        Console.WriteLine($"Enter the name of the meeting you wish to delete:");
                        string delName = Convert.ToString(Console.ReadLine());
                        var meetingsToDelete = JsonLoad(fileName);

                        foreach (Meeting m in meetingsToDelete)
                        {
                            if (delName == m.Name)
                            {
                                meetingsToDelete.Remove(m);
                                JsonWrite(fileName, meetingsToDelete);
                                break;
                            }
                        }
                        Console.WriteLine("Invalid name.");
                        break;
                    // add a person to a meeting
                    case "c":
                        Console.WriteLine($"Enter the name of the meeting you would like to add a person to:");
                        string meetingName = Convert.ToString(Console.ReadLine());
                        Console.WriteLine($"Enter the name of the person you would like to add:");
                        string personName = Convert.ToString(Console.ReadLine());
                        var meetingsAddPerson = JsonLoad(fileName);
                        foreach (Meeting m in meetingsAddPerson)
                        {
                            if (meetingName == m.Name)
                            {
                                if (!m.PersonList.Contains(personName))
                                {
                                    m.PersonList.Add(personName);
                                    JsonWrite(fileName, meetingsAddPerson);
                                    Console.WriteLine(personName + " has been successfully added to the meeting.");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("This person has already been added to this meeting.");
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Meeting " + meetingName + " does not exist.");
                                break;
                            }
                        }
                        break;
                    // remove a person from a meeting
                    case "d":
                        Console.WriteLine($"Enter the name of the meeting you would like to remove a person from:");
                        string remMeetingName = Convert.ToString(Console.ReadLine());
                        Console.WriteLine($"Enter the name of the person you wish to remove from the meeting:");
                        string remPersonName = Convert.ToString(Console.ReadLine());
                        var meetingsRemovePerson = JsonLoad(fileName);
                        foreach (Meeting m in meetingsRemovePerson)
                        {
                            if (remMeetingName == m.Name)
                            {
                                if (m.PersonList.Contains(remPersonName) & remPersonName != m.ResponsiblePerson)
                                {
                                    m.PersonList.Remove(remPersonName);
                                    JsonWrite(fileName, meetingsRemovePerson);
                                    Console.WriteLine("Person " + remPersonName + " has been successfully removed from the meeting.");
                                    break;
                                }
                                else if (m.PersonList.Contains(remPersonName) & remPersonName == m.ResponsiblePerson)
                                {
                                    Console.WriteLine("Could not remove person " + remPersonName + " from the meeting because they are responsible for this meeting.");
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Person " + remPersonName + " not found.");
                                break;
                            }
                        }
                        break;
                    // list meetings 
                    case "e":
                        var meetingsFilter = JsonLoad(fileName);
                        Console.WriteLine("List of currently available meetings: \n");

                        foreach (Meeting m in meetingsFilter)
                        {
                            Console.WriteLine("Name: " + m.Name);
                            Console.WriteLine("Description: " + m.Description);
                            Console.WriteLine("Type: " + m.Type);
                            Console.WriteLine("Category: " + m.Category);
                            Console.WriteLine("Start date: " + m.StartDate);
                            Console.WriteLine("End date: " + m.EndDate + "\n");
                        }
                        // filtering options
                        Console.WriteLine("Filter by:");
                        Console.WriteLine("\ta description");
                        Console.WriteLine("\tb responsible person");
                        Console.WriteLine("\tc category");
                        Console.WriteLine("\td type");
                        Console.WriteLine("\te date");
                        Console.WriteLine("\tf number of attendees");
                        Console.WriteLine("\tg back to main menu");

                        switch (Console.ReadLine())
                        {
                            // filtering by description
                            case "a":
                                {
                                    Console.WriteLine("Enter a key word to filter the description by:");
                                    string keyWord = Convert.ToString(Console.ReadLine());
                                    int c = 0;

                                    foreach (Meeting m in meetingsFilter)
                                    {
                                        if (m.Description.Contains(keyWord))
                                        {
                                            Console.WriteLine("Name: " + m.Name);
                                            Console.WriteLine("Description: " + m.Description);
                                            Console.WriteLine("Type: " + m.Type);
                                            Console.WriteLine("Category: " + m.Category);
                                            Console.WriteLine("Start date: " + m.StartDate);
                                            Console.WriteLine("End date: " + m.EndDate + "\n");
                                            c += 1;
                                        }
                                    }
                                    if (c == 0)
                                    { 
                                        Console.WriteLine("No meeting with such key word in the description exists."); 
                                    }
                                    break;
                                }
                            // filtering by responsible person
                            case "b":
                                {
                                    Console.WriteLine("Provide a name of the person to get a list of meetings they are responsible for:");
                                    string filterPerson = Convert.ToString(Console.ReadLine());
                                    int c = 0;

                                    foreach (Meeting m in meetingsFilter)
                                    {
                                        if (m.ResponsiblePerson == filterPerson)
                                        {
                                            Console.WriteLine("Name: " + m.Name);
                                            Console.WriteLine("Responsible person: " + m.ResponsiblePerson);
                                            Console.WriteLine("Description: " + m.Description);
                                            Console.WriteLine("Type: " + m.Type);
                                            Console.WriteLine("Category: " + m.Category);
                                            Console.WriteLine("Start date: " + m.StartDate);
                                            Console.WriteLine("End date: " + m.EndDate + "\n");
                                            c += 1;
                                        }
                                    }
                                    if (c == 0)
                                    {
                                        Console.WriteLine(filterPerson + " is currently not responsible for any meetings.");
                                    }
                                    break;
                                }
                            // filtering by category
                            case "c":
                                {
                                    Console.WriteLine("Select the category you would like to filter by:");
                                    Console.WriteLine("\ta CodeMonkey");
                                    Console.WriteLine("\tb Hub");
                                    Console.WriteLine("\tc Short");
                                    Console.WriteLine("\td TeamBuilding");

                                    switch (Console.ReadLine())
                                    {
                                        case "a":
                                            {
                                                int c = 0;
                                                foreach (Meeting m in meetingsFilter)
                                                {                                                
                                                    if (m.Category == "CodeMonkey")
                                                    {
                                                        Console.WriteLine("Name: " + m.Name);
                                                        Console.WriteLine("Description: " + m.Description);
                                                        Console.WriteLine("Type: " + m.Type);
                                                        Console.WriteLine("Category: " + m.Category);
                                                        Console.WriteLine("Start date: " + m.StartDate);
                                                        Console.WriteLine("End date: " + m.EndDate + "\n");
                                                        c += 1;
                                                    }
                                                }
                                                if (c == 0)
                                                {
                                                    Console.WriteLine("There are currently no meetings in this category.");
                                                }
                                                break;
                                            }
                                        case "b":
                                            {
                                                int c = 0;
                                                foreach (Meeting m in meetingsFilter)
                                                {
                                                    if (m.Category == "Hub")
                                                    {
                                                        Console.WriteLine("Name: " + m.Name);
                                                        Console.WriteLine("Description: " + m.Description);
                                                        Console.WriteLine("Type: " + m.Type);
                                                        Console.WriteLine("Category: " + m.Category);
                                                        Console.WriteLine("Start date: " + m.StartDate);
                                                        Console.WriteLine("End date: " + m.EndDate + "\n");
                                                        c += 1;
                                                    }
                                                }
                                                if (c == 0)
                                                {
                                                    Console.WriteLine("There are currently no meetings in this category.");
                                                }
                                                break;
                                            }
                                        case "c":
                                            {
                                                int c = 0;
                                                foreach (Meeting m in meetingsFilter)
                                                {
                                                    if (m.Category == "Short")
                                                    {
                                                        Console.WriteLine("Name: " + m.Name);
                                                        Console.WriteLine("Description: " + m.Description);
                                                        Console.WriteLine("Type: " + m.Type);
                                                        Console.WriteLine("Category: " + m.Category);
                                                        Console.WriteLine("Start date: " + m.StartDate);
                                                        Console.WriteLine("End date: " + m.EndDate + "\n");
                                                        c += 1;
                                                    }
                                                }
                                                if (c == 0)
                                                {
                                                    Console.WriteLine("There are currently no meetings in this category.");
                                                }
                                                break;
                                            }
                                        case "d":
                                            {
                                                int c = 0;
                                                foreach (Meeting m in meetingsFilter)
                                                {
                                                    if (m.Category == "TeamBuilding")
                                                    {
                                                        Console.WriteLine("Name: " + m.Name);
                                                        Console.WriteLine("Description: " + m.Description);
                                                        Console.WriteLine("Type: " + m.Type);
                                                        Console.WriteLine("Category: " + m.Category);
                                                        Console.WriteLine("Start date: " + m.StartDate);
                                                        Console.WriteLine("End date: " + m.EndDate + "\n");
                                                        c += 1;
                                                    }
                                                }
                                                if (c == 0)
                                                {
                                                    Console.WriteLine("There are currently no meetings in this category.");
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            // filtering by type
                            case "d":
                                {
                                    Console.WriteLine("Select the type you would like to filter by:");
                                    Console.WriteLine("\ta Live");
                                    Console.WriteLine("\tb InPerson");

                                    switch (Console.ReadLine())
                                    {
                                        case "a":
                                            {
                                                int c = 0;
                                                foreach (Meeting m in meetingsFilter)
                                                {
                                                    if (m.Type == "Live")
                                                    {
                                                        Console.WriteLine("Name: " + m.Name);
                                                        Console.WriteLine("Description: " + m.Description);
                                                        Console.WriteLine("Type: " + m.Type);
                                                        Console.WriteLine("Category: " + m.Category);
                                                        Console.WriteLine("Start date: " + m.StartDate);
                                                        Console.WriteLine("End date: " + m.EndDate + "\n");
                                                        c += 1;
                                                    }
                                                }
                                                if (c == 0)
                                                {
                                                    Console.WriteLine("There are currently no meetings of this type.");
                                                }
                                                break;
                                            }
                                        case "b":
                                            {
                                                int c = 0;
                                                foreach (Meeting m in meetingsFilter)
                                                {
                                                    if (m.Type == "InPerson")
                                                    {
                                                        Console.WriteLine("Name: " + m.Name);
                                                        Console.WriteLine("Description: " + m.Description);
                                                        Console.WriteLine("Type: " + m.Type);
                                                        Console.WriteLine("Category: " + m.Category);
                                                        Console.WriteLine("Start date: " + m.StartDate);
                                                        Console.WriteLine("End date: " + m.EndDate + "\n");
                                                        c += 1;
                                                    }
                                                }
                                                if (c == 0)
                                                {
                                                    Console.WriteLine("There are currently no meetings of this type.");
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            // filtering by date
                            case "e":
                                {
                                    Console.WriteLine("Filter by date options:");
                                    Console.WriteLine("\ta meetings happening after a certain date");
                                    Console.WriteLine("\tb meetings happening in between certain dates");
                                    switch (Console.ReadLine())
                                    {
                                        case "a":
                                            {
                                                Console.WriteLine("Provide a date to see meetings that happen after it:");
                                                DateTimeOffset MinDate = Convert.ToDateTime(Console.ReadLine());
                                                int c = 0;
                                                foreach (Meeting m in meetingsFilter)
                                                {
                                                    if (m.StartDate > MinDate)
                                                    {
                                                        Console.WriteLine("Name: " + m.Name);
                                                        Console.WriteLine("Description: " + m.Description);
                                                        Console.WriteLine("Type: " + m.Type);
                                                        Console.WriteLine("Category: " + m.Category);
                                                        Console.WriteLine("Start date: " + m.StartDate);
                                                        Console.WriteLine("End date: " + m.EndDate + "\n");
                                                        c += 1;
                                                    }
                                                }
                                                if (c == 0)
                                                {
                                                    Console.WriteLine("There are currently no meetings happening after " + MinDate);
                                                }
                                                break;
                                            }
                                        case "b":
                                            {
                                                Console.WriteLine("Provide a start date:");
                                                DateTimeOffset startDate = Convert.ToDateTime(Console.ReadLine());
                                                Console.WriteLine("Provide an end date:");
                                                DateTimeOffset endDate = Convert.ToDateTime(Console.ReadLine());
                                                int c = 0;
                                                foreach (Meeting m in meetingsFilter)
                                                {
                                                    if (m.StartDate > startDate & m.EndDate < endDate)
                                                    {
                                                        Console.WriteLine("Name: " + m.Name);
                                                        Console.WriteLine("Description: " + m.Description);
                                                        Console.WriteLine("Type: " + m.Type);
                                                        Console.WriteLine("Category: " + m.Category);
                                                        Console.WriteLine("Start date: " + m.StartDate);
                                                        Console.WriteLine("End date: " + m.EndDate + "\n");
                                                        c += 1;
                                                    }
                                                }
                                                if (c == 0)
                                                {
                                                    Console.WriteLine("There are currently no meetings happening between " + startDate + " and " + endDate);
                                                }
                                                break;
                                            }
                                    }
                                    break;
                                }
                            // filtering by attendee count
                            case "f":
                                {
                                    Console.WriteLine("Enter a number to get a list of meetings with a higher amount of attendees than the provided number:");
                                    int attCount = Convert.ToInt16(Console.ReadLine());
                                    int c = 0;
                                    foreach (Meeting m in meetingsFilter)
                                    {
                                        if (m.PersonList.Count > attCount)
                                        {
                                            Console.WriteLine("Name: " + m.Name);
                                            Console.WriteLine("Attendee count: " + m.PersonList.Count);
                                            Console.WriteLine("Description: " + m.Description);
                                            Console.WriteLine("Type: " + m.Type);
                                            Console.WriteLine("Category: " + m.Category);
                                            Console.WriteLine("Start date: " + m.StartDate);
                                            Console.WriteLine("End date: " + m.EndDate + "\n");
                                            c += 1;
                                        }
                                    }
                                    if (c == 0)
                                    {
                                        Console.WriteLine("There are currently no meetings with an attendee count higher than " + attCount);
                                    }
                                    break;
                                }
                            case "g":
                                {
                                    break;
                                }
                        }
                        break;
                }
                // continue back to main menu
                Console.WriteLine("Enter 'y' if you would like to continue.");
                string input = Console.ReadLine();
                if (input != "y")
                {
                    break;
                }
            }
        }
    }
}
