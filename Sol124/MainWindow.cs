using System;
using Gtk;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class MainWindow : Gtk.Window
{

	int stickAroundFlag = 0;
	int chillFlag = 0;
	int trueFlag = 0;
	int falseFlag = 0;
	int linesOnConsole = 0;

	int printFlag = 0;
	String printDesc = "";
	String valueForPrintingVar = "";
	String noProblemo = "1";
	String iLied = "0";
	int answerFlag = 0;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		InitializeUI();
	}
	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected struct Lexemes
	{
		public String lexeme;
		public String type;

		public Lexemes(String name, String token)
		{
			lexeme = name;
			type = token;
		}
	}

	Dictionary<string, string> varDict = new Dictionary<string, string>();


	protected void InitializeUI()
	{
		TreeViewColumn token = new TreeViewColumn();
		TreeViewColumn definition = new TreeViewColumn();
		CellRendererText code = new CellRendererText();
		CellRendererText attr = new CellRendererText();

		TreeViewColumn varName = new TreeViewColumn();
		TreeViewColumn varValue = new TreeViewColumn();
		CellRendererText nameVar = new CellRendererText();
		CellRendererText valueVar = new CellRendererText();

		TextView console = new TextView();

		token.Title = "Token";
		treeview.InsertColumn(token, 0);
		definition.Title = "Definition";
		treeview.InsertColumn(definition, 1);

		varName.Title = "Variable Name";
		stTree.InsertColumn(varName, 0);
		varValue.Title = "Value";
		stTree.InsertColumn(varValue, 1);

		token.PackStart(code, true);
		definition.PackStart(attr, true);
		token.AddAttribute(code, "text", 0);
		definition.AddAttribute(attr, "text", 1);

		varName.PackStart(nameVar, true);
		varValue.PackStart(valueVar, true);
		varName.AddAttribute(nameVar, "text", 0);
		varValue.AddAttribute(valueVar, "text", 1);


	}
	protected void OnOpen(object sender, System.EventArgs e)
	{

		textview.Buffer.Text = "";

		FileChooserDialog chooser = new FileChooserDialog(
			"Select a file",
			this,
			FileChooserAction.Open,
			"Cancel", ResponseType.Cancel,
			"Open", ResponseType.Accept);

		if (chooser.Run() == (int)ResponseType.Accept)
		{
			System.IO.StreamReader file =
				System.IO.File.OpenText(chooser.Filename);

			textview.Buffer.Text = file.ReadToEnd();
			file.Close();
		}
		chooser.Destroy();
	}

	protected void Run(object sender, EventArgs a)
	{


		ListStore codestore = new ListStore(typeof(string), typeof(string));
		treeview.Model = codestore;

		ListStore varstore = new ListStore(typeof(string), typeof(string));
		stTree.Model = varstore;

		List<Lexemes> symboltable = new List<Lexemes>();
		symboltable.Add(new Lexemes("end", "end"));
		String file = textview.Buffer.Text;
		String[] texts = file.Split('\n'); //splits the code per line

		String storeLine = "";
		String lexemeType = "";
		String errorDesc = "";

		var errorFlag = 0;  //checks if there are errors
		var startFlag = 0;  //checks if the keyword IT'S SHOWTIME exists
		var endFlag = 0;    //checks if the keyword YOU HAVE BEEN TERMINATED exists

		for (int i = 0; i < texts.Length; i++)
		{
			while (String.Compare(texts[i].Trim(), "") != 0)
			{
				storeLine = MatchRegex(texts[i], codestore, ref lexemeType);
				texts[i] = texts[i].Substring(storeLine.Length); //remove the found keyword in the string

				if (String.Compare(storeLine, "Error") == 0)
				{
					commandLine.Buffer.Text = ("Line " + (i + 1) + ": Invalid lexeme" + "\n");
					break;
				}
				else
				{
					//symboltable.Add(new Lexemes("end", "end"));
					//symboltable.Add(new Lexemes(storeLine, lexemeType));

					//commandLine.Buffer.Text = (storeLine + lexemeType);
					symboltable.Add(new Lexemes(storeLine, lexemeType));

				}
				texts[i] = texts[i].Trim();
			}

			if (String.Compare(storeLine, "Error") == 0)
			{
				break;
			}
			else
			{
				//parser
				symboltable.Add(new Lexemes("end", "end"));
				//var j = 0;
				//if (j < symboltable.Count)
				//{
				//  //varstore.AppendValues("hey", "hey");
				//  Parser(symboltable, ref startFlag, ref endFlag,  ref j, ref errorFlag, ref errorDesc, varstore, varDict);
				//  //commandLine.Buffer.Text = (symboltable[j].lexeme + ":" + symboltable[j].type);
				//  j--;
				//  //j--;
				//}
			}

			//commandLine.Buffer.Text = symboltable.Count.ToString();

			//symboltable.Add(new Lexemes(storeLine, lexemeType));
			//var j = 2;


			//commandLine.Buffer.Text = (varDict[1].Value);

			if (errorFlag != 0)
			{
				commandLine.Buffer.Text = ("Line " + (i + 1) + ": " + errorDesc);
				break;
			}



		}

		var counter = 0;
		while (counter < symboltable.Count)
		{

			Parser(symboltable, ref startFlag, ref endFlag, ref counter, ref errorFlag, ref errorDesc, varstore, varDict);
			if (printFlag != 0)
			{
				//textview3.Buffer.Text = String.Concat(textview3.Buffer.Text, valDummy.Trim('\"'));
				commandLine.Buffer.Text = String.Concat(commandLine.Buffer.Text, printDesc.Trim(('\"')));
				printFlag = 0;
				printDesc = "";
				commandLine.Buffer.Text = String.Concat(commandLine.Buffer.Text, "\n");
			}
			if (errorFlag != 0)
			{
				commandLine.Buffer.Text = ("Line " + (counter + 1) + ": " + errorDesc);
				break;
			}

		}


	}

	protected String MatchRegex(String text, ListStore codestore, ref String lexemeType)
	{

		MatchCollection ITSSHOWTIME = Regex.Matches(text, @"^(IT[']S SHOWTIME)(?=\s|$)");
		foreach (Match match in ITSSHOWTIME)
		{
			lexemeType = "start of program keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection TERMINATED = Regex.Matches(text, @"^(YOU HAVE BEEN TERMINATED)(?=\s|$)");
		foreach (Match match in TERMINATED)
		{
			lexemeType = "end of program keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection STRINGS = Regex.Matches(text, @"^""[^""]*""(?=\s|$)");
		foreach (Match match in STRINGS)
		{
			lexemeType = "string identifier";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();
		}

		MatchCollection INTEGERS = Regex.Matches(text, @"^-?\d+$");
		foreach (Match match in INTEGERS)
		{
			lexemeType = "integer identifier";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();
		}

		MatchCollection MACROTRUE = Regex.Matches(text, @"^(@NO PROBLEMO)(?=\s|$)");
		foreach (Match match in MACROTRUE)
		{
			lexemeType = "macro";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();
		}

		MatchCollection MACROFALSE = Regex.Matches(text, @"^(@I LIED)(?=\s|$)");
		foreach (Match match in MACROFALSE)
		{
			lexemeType = "macro";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();
		}

		MatchCollection HEYCHRISTMASTREE = Regex.Matches(text, @"^HEY CHRISTMAS TREE(?=\s|$)");
		foreach (Match match in HEYCHRISTMASTREE)
		{
			lexemeType = "variable declaration keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection YOUSETUSUP = Regex.Matches(text, @"^YOU SET US UP(?=\s|$)");
		foreach (Match match in YOUSETUSUP)
		{
			lexemeType = "variable initialization keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection GETTOTHECHOPPER = Regex.Matches(text, @"^(GET TO THE CHOPPER)(?=\s|$)");
		foreach (Match match in GETTOTHECHOPPER)
		{
			lexemeType = "fetch variable skeyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection HEREISMYINVITATION = Regex.Matches(text, @"^(HERE IS MY INVITATION)(?=\s|$)");
		foreach (Match match in HEREISMYINVITATION)
		{
			lexemeType = "assignment keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection ENOUGHTALK = Regex.Matches(text, @"^(ENOUGH TALK)(?=\s|$)");
		foreach (Match match in ENOUGHTALK)
		{
			lexemeType = "end of commands keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection GETUP = Regex.Matches(text, @"^(GET UP)(?=\s|$)");
		foreach (Match match in GETUP)
		{
			lexemeType = "arithmetic operation keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection GETDOWN = Regex.Matches(text, @"^(GET DOWN)(?=\s|$)");
		foreach (Match match in GETDOWN)
		{
			lexemeType = "arithmetic operation keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection YOUREFIRED = Regex.Matches(text, @"^(YOU[']RE FIRED)(?=\s|$)");
		foreach (Match match in YOUREFIRED)
		{
			lexemeType = "arithmetic operation keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection HEHADTOSPLIT = Regex.Matches(text, @"^(HE HAD TO SPLIT)(?=\s|$)");
		foreach (Match match in HEHADTOSPLIT)
		{
			lexemeType = "arithmetic operation keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection YOUARENOTYOUYOUAREME = Regex.Matches(text, @"^(YOU ARE NOT YOU YOU ARE ME)(?=\s|$)");
		foreach (Match match in YOUARENOTYOUYOUAREME)
		{
			lexemeType = "logical operation keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection LETOFFSOMESTEAMBENNET = Regex.Matches(text, @"(^LET OFF SOME STEAM BENNET)(?=\s|$)");
		foreach (Match match in LETOFFSOMESTEAMBENNET)
		{
			lexemeType = "logical operation keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection CONSIDERTHATADIVORCE = Regex.Matches(text, @"(^CONSIDER THAT A DIVORCE)(?=\s|$)");
		foreach (Match match in CONSIDERTHATADIVORCE)
		{
			lexemeType = "logical operation keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection KNOCKKNOCK = Regex.Matches(text, @"(^KNOCK KNOCK)(?=\s|$)");
		foreach (Match match in KNOCKKNOCK)
		{
			lexemeType = "logical operation keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}


		MatchCollection TALKTOTHEHAND = Regex.Matches(text, @"(^TALK TO THE HAND)(?=\s|$)");
		foreach (Match match in TALKTOTHEHAND)
		{
			lexemeType = "print keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection GETYOURASSTOMARS = Regex.Matches(text, @"(^GET YOUR ASS TO MARS)(?=\s|$)");
		foreach (Match match in GETYOURASSTOMARS)
		{
			lexemeType = "output assignment keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection DOITNOW = Regex.Matches(text, @"(^DO IT NOW)(?=\s|$)");
		foreach (Match match in DOITNOW)
		{
			codestore.AppendValues(match.ToString(), "initialization of input keyword");
			return match.ToString();

		}

		MatchCollection IWANTTOASK = Regex.Matches(text, @"(^I WANT TO ASK YOU A BUNCH OF QUESTIONS AND I WANT TO HAVE THEM ANSWERED IMMEDIATELY)(?=\s|$)");
		foreach (Match match in IWANTTOASK)
		{
			lexemeType = "input keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}
		MatchCollection BULLSHIT = Regex.Matches(text, @"(^BULLSHIT)(?=\s|$)");
		foreach (Match match in BULLSHIT)
		{
			lexemeType = "keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection BECAUSEIMGOINGTOSAYPLEASE = Regex.Matches(text, @"(^BECAUSE I[']M GOING TO SAY PLEASE)(?=\s|$)");
		foreach (Match match in BECAUSEIMGOINGTOSAYPLEASE)
		{
			lexemeType = "keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection YOUHAVENORESPECTFORLOGIC = Regex.Matches(text, @"(^YOU HAVE NO RESPECT FOR LOGIC)(?=\s|$)");
		foreach (Match match in YOUHAVENORESPECTFORLOGIC)
		{
			lexemeType = "keyword";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		MatchCollection STICKAROUND = Regex.Matches(text, @"(^STICK AROUND)(?=\s|$)");
		foreach (Match match in STICKAROUND)
		{
			stickAroundFlag += 1;
			codestore.AppendValues(match.ToString(), "keyword");
			return match.ToString();

		}

		MatchCollection CHILL = Regex.Matches(text, @"(^CHILL)(?=\s|$)");
		foreach (Match match in CHILL)
		{
			chillFlag += 1;
			codestore.AppendValues(match.ToString(), "keyword");
			return match.ToString();

		}

		MatchCollection IDENTIFIERS = Regex.Matches(text, @"^[a-zA-Z][a-z|A-Z|_|\d]*(?=\s|$)");
		foreach (Match match in IDENTIFIERS)
		{
			lexemeType = "identifier";
			codestore.AppendValues(match.ToString(), lexemeType);
			return match.ToString();

		}

		return "Error";

	}

	protected void Parser(List<Lexemes> symboltable, ref int startFlag, ref int endFlag, ref int i, ref int errorFlag, ref String errorDesc, ListStore varstore, Dictionary<String, String> varDict)
	{
		while (i < symboltable.Count)
		{
			if (symboltable[i].lexeme == "end")
			{
				i++;
				continue;
			}
			else if (symboltable[i].lexeme == "IT'S SHOWTIME")
			{
				startFlag++;
				i++;


				//if (symboltable[i].lexeme == "end")
				//{
				//  //var holder = symboltable.Count;
				//  ///holder--;
				//  //if (symboltable[holder].lexeme == "YOU HAVE BEEN TERMINATED")
				//  //{
				//  //  i++;
				//  //  break;
				//  //}
				//  //else
				//  //{
				//  //  errorFlag += 1;
				//  //  errorDesc = "expected: end of program keyword";
				//  //}
				//  i++;
				//  break;
				//}
				//else
				//{
				//  errorFlag += 1;
				//  errorDesc = "expected: EOL";
				//  break;
				//}

			}
			else if (symboltable[symboltable.Count - 1].lexeme == "YOU HAVE BEEN TERMINATED" && endFlag == 0)
			{
				endFlag += 1;
				i++;
				if (startFlag == 0)
				{
					errorFlag += 1;
					errorDesc = "not found: program initialization keyword";
					break;
				}
				else if (symboltable[i].lexeme == "end")
				{
					i++;
					break;
				}
				else
				{
					errorFlag += 1;
					errorDesc = "expected: EOF";
					break;
				}

			}
			else if (true)
			{
				keywordGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
				if (errorFlag > 0)
				{
					break;
				}
				else
				{
					i++;
					break;
				}
			}
			else
			{
				errorDesc = symboltable[i].lexeme;
				errorFlag += 1;
				break;
			}

		}

	}

	protected void keywordGrammar(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ListStore varstore, Dictionary<String, String> varDict)
	{
		if (symboltable[i].lexeme == "HEY CHRISTMAS TREE")
		{
			varDecGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
			return;
		}
		else if (symboltable[i].lexeme == "GET TO THE CHOPPER")
		{
			varReGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
			return;
			//inside this grammar^ should exist the arithmeticGrammar() and logicalGrammar()
			//since, reassignment statements are required 
			//also expressionsGrammar()
		}
		else if (symboltable[i].lexeme == "TALK TO THE HAND")
		{
			printGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
			return;
		}
		else if (symboltable[i].lexeme == "GET YOUR ASS TO MARS")
		{
			inputGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
			return;
		}
		else if (symboltable[i].lexeme == "BECAUSE I'M GOING TO SAY PLEASE")
		{
			ifElseGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
			return;
		}
		else if (symboltable[i].lexeme == "STICK AROUND")
		{
			loopGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
			return;
		}
		else if (symboltable[i].lexeme == "YOU HAVE BEEN TERMINATED")
		{
			return;
		}
        else if(symboltable[i].lexeme == "BULLSHIT")
        {
            return;
        }
        else if(symboltable[i].lexeme == "YOU HAVE NO RESPECT FOR LOGIC")
        {
            return;
        }
		else
		{
			errorDesc = symboltable[i].lexeme;
			errorFlag += 1;
			return;
		}
	}

	protected int typeChecker(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ref String identifierType)
	{
		if (symboltable[i].type == identifierType)
		{
			var z = 0;
			Int32.TryParse(symboltable[i - 3].lexeme, out z);
			return z;
		}
		else
		{
			errorFlag += 1;
			errorDesc = "error: mismatch type";
			return 0;
		}
	}



	protected void logicSolver(ref int x, ref int y, ref String operation)
	{
		switch (operation)
		{
			case "equalsTo":
				if (x == y)
				{
					trueFlag += 1;
					return;
				}
				else
				{
					falseFlag += 1;
					return;
				}
			case "greaterThan":
				if (x < y)
				{
					trueFlag += 1;
					return;
				}
				else
				{
					falseFlag += 1;
					return;
				}
			case "or":
				if (x > 0 || y > 0)
				{
					trueFlag += 1;
					return;
				}
				else
				{
					falseFlag += 1;
					return;
				}
			case "and":
				if (x > 0 && y > 0)
				{
					trueFlag += 1;
					return;
				}
				else
				{
					falseFlag += 1;
					return;
				}
			default:
				break;


		}

	}

	protected void arithmeticSolver(ref int x, ref int y, ref String operation)
	{
		switch (operation)
		{
			case "add":
				answerFlag = x + y;
				return;
			case "minus":
				answerFlag = y - x;
				return;
			case "multiply":
				answerFlag = x * y;
				return;
			case "divide":
				answerFlag = y / x;
				return;
			default:
				break;
		}
	}

	protected void logicalGrammarHelper(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ref String logicType, int secondValue)
	{
		i++;
		var x = 0;
		var y = 0;
		String identifierType;
		String valueOfVar;
		int y_value = secondValue + 1;

		if (symboltable[i].type == "macro")
		{
			Int32.TryParse(symboltable[i].lexeme, out x);
			identifierType = "macro";
			y = typeChecker(symboltable, ref i, ref errorFlag, ref errorDesc, ref identifierType);
			logicSolver(ref x, ref y, ref logicType);
			i++;
			return;
		}
		else if (symboltable[i].type == "identifier")
		{
			String value = "";
			if (varDict.ContainsKey(symboltable[i].lexeme))
			{
				value = varDict[symboltable[i].lexeme];
				Int32.TryParse(value, out x);
				identifierType = "identifier";
				if (varDict.ContainsKey(symboltable[y_value].lexeme))
				{

					valueOfVar = varDict[symboltable[y_value].lexeme];
				}
				else
				{
					errorFlag += 1;
					errorDesc = "error: variable does not exist";
					return;
				}
				Int32.TryParse(valueOfVar, out y);
				logicSolver(ref x, ref y, ref logicType);
				i++;
				return;
			}
			else
			{
				Console.WriteLine("variable does not exist");
			}
			//holder = symboltable[i - 1].lexeme;

		}
		else if (symboltable[i].type == "integer identifier")
		{
			Int32.TryParse(symboltable[i].lexeme, out x);
			identifierType = "integer identifier";
			y = typeChecker(symboltable, ref i, ref errorFlag, ref errorDesc, ref identifierType);
			logicSolver(ref x, ref y, ref logicType);
			i++;
			return;
		}
		else
		{
			errorFlag += 1;
			errorDesc = "invalid type. expected type: macro or int";
			return;
		}
		return;
	}

	protected void arithmeticGrammarHelper(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ref String arithmeticType, int secondValue)
	{
		i++;
		var x = 0;
		var y = 0;
		String identifierType;
		String valueOfVar;
		int y_value = secondValue + 1;

		if (symboltable[i].type == "identifier")
		{
			String value = "";
			if (varDict.ContainsKey(symboltable[i].lexeme))
			{
				value = varDict[symboltable[i].lexeme];
				Int32.TryParse(value, out x);
				identifierType = "identifier";
				if (varDict.ContainsKey(symboltable[y_value].lexeme))
				{

					valueOfVar = varDict[symboltable[y_value].lexeme];
				}
				else
				{
					errorFlag += 1;
					errorDesc = "error: variable does not exist";
					return;
				}
				Int32.TryParse(valueOfVar, out y);
				arithmeticSolver(ref x, ref y, ref arithmeticType);
				i++;
				return;
			}
			else
			{
				Console.WriteLine("variable does not exist");
			}
			//holder = symboltable[i - 1].lexeme;

		}
		else if (symboltable[i].type == "integer identifier")
		{
			Int32.TryParse(symboltable[i].lexeme, out x);
			identifierType = "integer identifier";
			y = typeChecker(symboltable, ref i, ref errorFlag, ref errorDesc, ref identifierType);
			arithmeticSolver(ref x, ref y, ref arithmeticType);
			i++;
			return;
		}
		else
		{
			errorFlag += 1;
			errorDesc = "invalid type. expected type: macro or int";
			return;
		}
		return;
	}

	protected void logicalGrammar(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ListStore varstore, int secondValue)
	{
		if (symboltable[i].lexeme == "YOU ARE NOT YOU YOU ARE ME")
		{
			String logicType = "equalsTo";
			logicalGrammarHelper(symboltable, ref i, ref errorFlag, ref errorDesc, ref logicType, secondValue);
			return;
		}
		else if (symboltable[i].lexeme == "LET OFF SOME STEAM BENNET")
		{
			String logicType = "greaterThan";
			logicalGrammarHelper(symboltable, ref i, ref errorFlag, ref errorDesc, ref logicType, secondValue);
			return;
		}
		else if (symboltable[i].lexeme == "CONSIDER THAT A DIVORCE")
		{
			String logicType = "or";
			logicalGrammarHelper(symboltable, ref i, ref errorFlag, ref errorDesc, ref logicType, secondValue);
			return;
		}
		else if (symboltable[i].lexeme == "KNOCK KNOCK")
		{
			String logicType = "and";
			logicalGrammarHelper(symboltable, ref i, ref errorFlag, ref errorDesc, ref logicType, secondValue);
			return;
		}
	}

	protected void arithmeticGrammar(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ListStore varstore, int secondValue)
	{
		if (symboltable[i].lexeme == "GET UP")
		{
			String arithmeticType = "add";
            arithmeticGrammarHelper(symboltable, ref i, ref errorFlag, ref errorDesc, ref arithmeticType,secondValue);
			return;
		}
		else if (symboltable[i].lexeme == "GET DOWN")
		{
			String arithmeticType = "minus";
			arithmeticGrammarHelper(symboltable, ref i, ref errorFlag, ref errorDesc, ref arithmeticType, secondValue);
			return;
		}
		else if (symboltable[i].lexeme == "YOU'RE FIRED")
		{
			String arithmeticType = "multiply";
			arithmeticGrammarHelper(symboltable, ref i, ref errorFlag, ref errorDesc, ref arithmeticType, secondValue);
			return;
		}
		else if (symboltable[i].lexeme == "HE HAD TO SPLIT")
		{
			String arithmeticType = "divide";
			arithmeticGrammarHelper(symboltable, ref i, ref errorFlag, ref errorDesc, ref arithmeticType, secondValue);
			return;
		}
	}


	protected void varDecGrammar(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ListStore varstore, Dictionary<String, String> varDict)
	{
		i++;
		if (symboltable[i].type == "identifier")
		{
			i++;
			if (symboltable[i].type == "end")
			{
				i++;
				if (symboltable[i].lexeme == "YOU SET US UP")
				{
					i++;
					if (symboltable[i].type == "integer identifier" || symboltable[i].type == "macro")
					{
						i++;
						if (symboltable[i].type == "end")
						{
							varstore.AppendValues(symboltable[i - 4].lexeme, symboltable[i - 1].lexeme);
							varDict.Add(symboltable[i - 4].lexeme, symboltable[i - 1].lexeme);
							return;
						}
						else
						{
							errorFlag += 1;
							errorDesc = "expected: EOL";
							return;
						}
					}
					else
					{
						errorFlag += 1;
						errorDesc = "invalid type. expected: integer or macro";
						return;
					}

				}
				else
				{
					errorFlag += 1;
					errorDesc = "expected: YOU SET US UP";
					return;

				}

			}
			else
			{
				errorFlag += 1;
				errorDesc = "expected: EOL";
				return;
			}
		}
		else
		{
			errorFlag += 1;
			errorDesc = "expected: identifier";
			return;

		}
	}

	protected void varReGrammar(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ListStore varstore, Dictionary<String, String> varDict)
	{
		i++;
		if (symboltable[i].type == "identifier")
		{
			i++;
			if (symboltable[i].type == "end")
			{
				i++;
				if (symboltable[i].lexeme == "HERE IS MY INVITATION")
				{
					i++;
					if (symboltable[i].type == "integer identifier" || symboltable[i].type == "macro" || symboltable[i].type == "identifier")
					{
						i++;
						if (symboltable[i].type == "end")
						{
							i++;
							if (symboltable[i].lexeme == "ENOUGH TALK")
							{
								i++;
								if (symboltable[i].type == "end")
								{
									if (varDict.ContainsKey(symboltable[i - 6].lexeme))
									{
										if (symboltable[i - 3].type == "identifier")
										{
											String valueOfVar = "";
											if (varDict.ContainsKey(symboltable[i - 3].lexeme))
											{

												valueOfVar = varDict[symboltable[i].lexeme];

												varDict.Remove(symboltable[i - 6].lexeme);
												varDict.Add(symboltable[i - 6].lexeme, valueOfVar);
												varstore.AppendValues(symboltable[i - 6].lexeme, valueOfVar);
												return;
											}
											else
											{
												errorFlag += 1;
												errorDesc = "error: variable does not exist";
												return;
											}

										}
										else
										{
											varDict.Remove(symboltable[i - 6].lexeme);
											varDict.Add(symboltable[i - 6].lexeme, symboltable[i - 3].lexeme);
											varstore.AppendValues(symboltable[i - 6].lexeme, symboltable[i - 3].lexeme);
											return;
										}

									}
									else
									{
										errorFlag += 1;
										errorDesc = "error: variable does not exist";
										return;
									}
									//bind variable

								}
								else
								{
									errorFlag += 1;
									errorDesc = "expected: EOL";
									return;
								}
								//varDict.Add(new KeyValuePair<string, string>(symboltable[i - 1].lexeme, symboltable[i + 1].lexeme));
							}

							else if (symboltable[i].type == "arithmetic operation keyword")
							{
								arithmeticGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, i - 3);
								i++;
								if (symboltable[i].lexeme == "ENOUGH TALK")
								{
									varDict.Remove(symboltable[i - 8].lexeme);
									varDict.Add(symboltable[i - 8].lexeme, answerFlag.ToString());
									Console.WriteLine(answerFlag.ToString());
									varstore.AppendValues(symboltable[i - 8].lexeme, answerFlag.ToString());
								}
								return;
							}
							else if (symboltable[i].type == "logical operation keyword")
							{
								if (symboltable[i - 3].type == "identifier")
								{
									logicalGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, i - 3);
									i++;
								}
								else
								{
									logicalGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, i - 3);
									i++;
								}
								if (trueFlag > 0)
								{
									varDict.Remove(symboltable[i - 8].lexeme);
									varDict.Add(symboltable[i - 8].lexeme, "1");
									varstore.AppendValues(symboltable[i - 8].lexeme, "1");
								}
								else if (falseFlag > 0)
								{
									varDict.Remove(symboltable[i - 8].lexeme);
									varDict.Add(symboltable[i - 8].lexeme, "0");
									varstore.AppendValues(symboltable[i - 8].lexeme, "0");
								}
								else
								{

								}
								return;
							}
							else
							{
								errorFlag += 1;
								errorDesc = "expected: keyword ENOUGH TALK or operations";
								return;
							}
						}
						else
						{
							errorFlag += 1;
							errorDesc = "expected: EOL";
							return;
						}
					}
					else
					{
						errorFlag += 1;
						errorDesc = "invalid type. expected: macro or integer";
						return;
					}
				}
				else
				{
					errorFlag += 1;
					errorDesc = "expected: keyword HERE IS MY INVITATION";
					return;
				}
			}
			else
			{
				errorFlag += 1;
				errorDesc = "expected: EOL";
				return;
			}
		}
		else
		{
			errorFlag += 1;
			errorDesc = "expected: identifier";
			return;
		}

	}

	protected void loopGrammar(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ListStore varstore, Dictionary<String, String> varDic)
	{
		if (chillFlag > 0)
		{
			i++;
			if (symboltable[i].type == "macro")
			{
				bool loopMacro = true;
				while (loopMacro == true)
				{
					if (symboltable[i].lexeme == "@NO PROBLEMO")
					{
						i++;
						keywordGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
					}
					else if (symboltable[i].lexeme == "@I LIED")
					{
						loopMacro = false;
					}
					else
					{
						errorFlag += 1;
						errorDesc = "invalid type. expected: macro";
						return;
					}
				}
			}
			else if (symboltable[i].type == "identifier")
			{
				var loopStatement = 0;
				String value = "";
				if (varDict.ContainsKey(symboltable[i].lexeme))
				{
					value = varDict[symboltable[i].lexeme];

					if (value == "@NO PROBLEMO" || value == "@I LIED")
					{

					}
					else
					{

						var loopStart = i;
						var loopCounter = 0;
						bool endOfLoop = true;

						Int32.TryParse(value, out loopStatement);
						while (loopStatement > 0)
						{
							loopCounter = loopStart;
							while (endOfLoop)
							{
								loopCounter++;

								if (symboltable[i].lexeme == "CHILL")
								{
									endOfLoop = false;

								}
								i++;
								keywordGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
							}
							loopStatement--;
						}
						i = loopCounter;
						i++;
						return;
					}
				}
				else
				{
					errorFlag += 1;
					errorDesc = "variable does not exist";
					return;
				}

			}
			else
			{
				errorFlag += 1;
				errorDesc = "invalid type. expected: macro";
				return;
			}
		}
		else
		{
			errorFlag += 1;
			errorDesc = "no CHILL keyword";
			return;
		}
	}

	protected void ifElseGrammar(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ListStore varstore, Dictionary<String, String> varDict)
	{
		i++;
		if (symboltable[i].type == "identifier")
		{
			if (varDict.ContainsKey(symboltable[i].lexeme))
			{
                if (varDict[symboltable[i].lexeme] == "I LIED" || varDict[symboltable[i].lexeme] == "0")
				{
                    i++;
                    if(symboltable[i].type == "end"){
                        i++;
                        if(symboltable[i].lexeme == "BULLSHIT"){
                            i += 2;
							while (symboltable[i].lexeme != "YOU HAVE NO RESPECT FOR LOGIC")
							{
								keywordGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
								i++;
								if (symboltable[i].type == "end")
								{
									i++;
								}
							}

						}
                        else if(symboltable[i].lexeme == "YOU HAVE NO RESPECT FOR LOGIC"){
                            i += 2;
                        }
                    }
				}
                else if ((varDict[symboltable[i].lexeme] == "NO PROBLEMO") || (varDict[symboltable[i].lexeme] == "1"))
				{
					i++;
					if (symboltable[i].type == "end")
					{
						i++;
						while (symboltable[i].lexeme != "YOU HAVE NO RESPECT FOR LOGIC")
						{
							keywordGrammar(symboltable, ref i, ref errorFlag, ref errorDesc, varstore, varDict);
							i++;
							if (symboltable[i].type == "end")
							{
								i++;
							}
						}
					}

				}
			}
		}
		return;
	}

	protected void printGrammar(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ListStore varstore, Dictionary<String, String> varDic)
	{
		i++;
		if (symboltable[i].type == "integer" | symboltable[i].type == "string identifier")
		{
			printFlag += 1;
			printDesc = symboltable[i].lexeme;
			linesOnConsole += 1;
			return;
		}
		else if (symboltable[i].type == "macro")
		{

			if (symboltable[i].lexeme == "NO PROBLEMO")
			{
				printFlag += 1;
				printDesc = noProblemo;
				linesOnConsole += 1;
			}
			else if (symboltable[i].lexeme == "I LIED")
			{
				printFlag += 1;
				printDesc = iLied;
				linesOnConsole += 1;
			}
			return;
		}
		else if (symboltable[i].type == "identifier")
		{
			if (varDict.ContainsKey(symboltable[i].lexeme))
			{
				if (varDict[symboltable[i].lexeme] == "I LIED")
				{
					valueForPrintingVar = iLied;
				}
				else if (varDict[symboltable[i].lexeme] == "NO PROBLEMO")
				{
					valueForPrintingVar = noProblemo;
				}
				else
				{
					valueForPrintingVar = varDict[symboltable[i].lexeme];
				}
			}
			else
			{
				errorFlag += 1;
				errorDesc = "variable does not exist";
				return;
			}
			printFlag += 1;
			printDesc = valueForPrintingVar;
			linesOnConsole += 1;
			return;

		}
		else
		{
			errorFlag += 1;
			errorDesc = "invalid type. expected: macro, integer, string, or variable";
			linesOnConsole += 1;
			return;
		}


	}

	protected void inputGrammar(List<Lexemes> symboltable, ref int i, ref int errorFlag, ref String errorDesc, ListStore varstore, Dictionary<String, String> varDic)
	{
		i++;
		if (symboltable[i].type == "identifier")
		{
			i++;
			if (symboltable[i].type == "end")
			{
				i++;
				if (symboltable[i].lexeme == "DO IT NOW")
				{
					i++;
					if (symboltable[i].type == "end")
					{
						i++;
						if (symboltable[i].type == "user input keyword")
						{
							String userInput = commandLine.Buffer.Text;
							String[] textsOnConsole = userInput.Split('\n');

							varDict.Add(symboltable[i - 4].lexeme, textsOnConsole[0]);
							varstore.AppendValues(symboltable[i - 4].lexeme, textsOnConsole[0]);
						}
						else
						{
							errorFlag += 1;
							errorDesc = "expected: user input keyword";
							return;
						}
					}
					else
					{
						errorFlag += 1;
						errorDesc = "expected: EOL";
						return;
					}

				}
				else
				{
					errorFlag += 1;
					errorDesc = "expected: DO IT NOW keyword";
					return;
				}
			}
			else
			{
				errorFlag += 1;
				errorDesc = "expected: EOL";
				return;
			}
		}
		else
		{
			errorFlag += 1;
			errorDesc = "invalid type. expected: variable";
			return;
		}

	}



}
