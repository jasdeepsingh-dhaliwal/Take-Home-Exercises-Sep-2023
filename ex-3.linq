<Query Kind="Program" />

void Main()
{


	#region Driver  //  3 Marks
	try
	{
		// Task 1
		EmployeeRegistrationView newEmployee = new EmployeeRegistrationView();
		newEmployee.EmployeeID = 3;
		newEmployee.FirstName = GenerateName(4);
		newEmployee.LastName = GenerateName(7);
		newEmployee.HomePhone = RandomPhoneNumber();
		newEmployee.EmployeeSkills = new List<EmployeeSkillView>
		{
			new EmployeeSkillView
			{
				SkillID = 1,
				Level = 3,
				YearsOfExperience = 5,
				HourlyWage = 25.0m
			},
			new EmployeeSkillView
			{
				SkillID = 2,
				Level = 2,
				YearsOfExperience = 2,
				HourlyWage = 20.0m
			}
		};

		newEmployee.Dump("New Employee Add");

		EmployeeRegistrationView afterAdd = AddEditEmployeeRegistration(newEmployee);
				

		Console.WriteLine("-----------------------------------------------------");


		// Task 2: Update an employee and their skill list.

		EmployeeRegistrationView existingEmployee = new EmployeeRegistrationView();
		existingEmployee.EmployeeID = 123;
		existingEmployee.FirstName = GenerateName(5);
		existingEmployee.LastName = GenerateName(3);
		existingEmployee.HomePhone = "555-555-5555";
		existingEmployee.EmployeeSkills = new List<EmployeeSkillView>
		{
			new EmployeeSkillView
			{
				EmployeeSkillID = 5,
				SkillID = 3,
				Level = 4,
				YearsOfExperience = 3,
				HourlyWage = 30.0m
			},
			new EmployeeSkillView
			{
				EmployeeSkillID = 6,
				SkillID = 4,
				Level = 2,
				YearsOfExperience = 2,
				HourlyWage = 18.0m
			}
		};
		
		existingEmployee.Dump("After Edit");
		EmployeeRegistrationView afterEdit = AddEditEmployeeRegistration(existingEmployee);
		
		Console.WriteLine("-----------------------------------------------------------");

		//  Task 3 attempts to register new skills with invalid data that will trigger all the business in this exercise
		EmployeeRegistrationView invalidEmployee = new EmployeeRegistrationView
		{
			FirstName = "Brandon",
			LastName = "",
			HomePhone = "555-555-5555",
			EmployeeSkills = new List<EmployeeSkillView>
		{
			new EmployeeSkillView
			{
				SkillID = 5, 
                Level = -1, 
                YearsOfExperience = 200,
                HourlyWage = 10.0m 
            }
		}
		};

		AddEditEmployeeRegistration(invalidEmployee);
		
		invalidEmployee.Dump("After Correction");
		EmployeeRegistrationView afterCorrection = AddEditEmployeeRegistration(invalidEmployee);
	}
	#endregion

	#region catch all exceptions 
	catch (AggregateException ex)
	{
		foreach (var error in ex.InnerExceptions)
		{
			error.Message.Dump();
		}
	}
	catch (ArgumentNullException ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	catch (Exception ex)
	{
		GetInnerException(ex).Message.Dump();
	}
	#endregion
}
private Exception GetInnerException(Exception ex)
{
	while (ex.InnerException != null)
		ex = ex.InnerException;
	return ex;
}



#region Methods

#region AddEditEmployeeRegistration Method   //  6 Marks
public EmployeeRegistrationView AddEditEmployeeRegistration(EmployeeRegistrationView employeeRegistration)
{
	// --- Business Logic and Parameter Exception Section --- 
	#region Business Logic and Parameter Exception  //  2 Marks
	List<Exception> errorList = new List<Exception>();

	if (string.IsNullOrWhiteSpace(employeeRegistration.FirstName))
	{
		throw new ArgumentNullException("First name is required.");
	}
	
	
	if (string.IsNullOrWhiteSpace(employeeRegistration.LastName))
	{
		throw new ArgumentNullException("Last name is required.");
	}

	if (string.IsNullOrWhiteSpace(employeeRegistration.HomePhone))
	{
		throw new ArgumentNullException("Home Phone is required.");
	}

	if (employeeRegistration.EmployeeSkills == null || employeeRegistration.EmployeeSkills.Count == 0)
	{
		throw new ArgumentException("At least one new valid skill must be added.");
	}
	
	#endregion

	// --- Main Method Logic Section --- 
	#region Method Code //  3 Marks
	
	employeeRegistration.Active = true;

	foreach (var skill in employeeRegistration.EmployeeSkills)
	{
		if (skill.Level <= 0)
		{
			throw new ArgumentException("A valid 'Level' is required.");
		}

		if (skill.YearsOfExperience.HasValue)
		{
			if (skill.YearsOfExperience < 1 || skill.YearsOfExperience > 50)
			{
				throw new ArgumentException("Years of Experience (YOE) must be between 1 and 50.");
			}
		}

		if (skill.HourlyWage <= 0 || skill.HourlyWage < 15.00m || skill.HourlyWage > 100.00m)
		{
			throw new ArgumentException("Hourly Wage should be within the range of $15.00 to $100.00.");
		}
	}

	#endregion

	#region Check for errors and saving of data //  1 Marks
	if (errorList.Count() > 0)
	{
		throw new AggregateException("Unable to proceed!", errorList);
	}
	#endregion
	return null;
}
#endregion

#region GetEmployeeRegistration Method    //  1 Marks

#endregion

#endregion

/// <summary> 
/// Contains class definitions that are referenced in the current LINQ file. 
/// </summary> 
/// <remarks> 
/// It's crucial to highlight that in standard development practices, code and class definitions  
/// should not be mixed in the same file. Proper separation of concerns dictates that classes  
/// should have their own dedicated files, promoting modularity and maintainability. 
/// </remarks> 
#region Class/View Model   

public class EmployeeRegistrationView
{
	public int EmployeeID { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string HomePhone { get; set; }
	public bool Active { get; set; }
	public List<EmployeeSkillView> EmployeeSkills { get; set; } = new();
}

public class EmployeeSkillView
{
	public int EmployeeSkillID { get; set; }
	public int EmployeeID { get; set; }
	public int SkillID { get; set; }
	public int Level { get; set; }
	public int? YearsOfExperience { get; set; }
	public decimal HourlyWage { get; set; }
}

#endregion

#region Supporting Methods
/// <summary>
/// Generates a random phone number.
/// The generated phone number ensures the first digit is not 0 or 1.
/// </summary>
/// <returns>A random phone number.</returns>
public static string RandomPhoneNumber()
{
	var random = new Random();
	string phoneNumber = string.Empty;

	// Ensure the first digit isn't 0 or 1.
	int firstDigit = random.Next(2, 10); // Generates a random digit between 2 and 9.
	phoneNumber = $"{firstDigit}";

	// Generate the rest of the digits.
	for (int i = 1; i < 10; i++)
	{
		int currentDigit = random.Next(10);
		phoneNumber = $"{phoneNumber}{currentDigit}";

		// Add periods after every third digit (except for the last period).
		if (i % 3 == 2 && i != 8)
		{
			phoneNumber = $"{phoneNumber}.";
		}
	}

	return phoneNumber;
}

/// <summary>
/// Generates a random name of a given length.
/// The generated name follows a pattern of alternating consonants and vowels.
/// </summary>
/// <param name="len">The desired length of the generated name.</param>
/// <returns>A random name of the specified length.</returns>
public static string GenerateName(int len)
{
	// Create a new Random instance.
	Random r = new Random();

	// Define consonants and vowels to use in the name generation.
	string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
	string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };

	string Name = "";

	// Start the name with an uppercase consonant and a vowel.
	Name += consonants[r.Next(consonants.Length)].ToUpper();
	Name += vowels[r.Next(vowels.Length)];

	// Counter for tracking the number of characters added.
	int b = 2;

	// Add alternating consonants and vowels until we reach the desired length.
	while (b < len)
	{
		Name += consonants[r.Next(consonants.Length)];
		b++;
		Name += vowels[r.Next(vowels.Length)];
		b++;
	}

	return Name;
}
#endregion