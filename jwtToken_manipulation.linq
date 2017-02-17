<Query Kind="Program" />

void Main()
{
	var payload = File.ReadAllText(@"F:\Szkolenie\jwt_token.txt");
	var converted = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload)).Trim('=');
	converted.Dump();
}