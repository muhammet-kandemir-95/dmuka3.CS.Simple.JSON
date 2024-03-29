# dmuka3.CS.Simple.JSON

This library provides you to use JSON in C# like JavaScript.
 
 ## Nuget
 **Link** : https://www.nuget.org/packages/dmuka3.CS.Simple.JSON
 ```nuget
 Install-Package dmuka3.CS.Simple.JSON
 ```
 
 ## Example 1
 
  We will use library with dynamic variables.
 
 ```csharp
var developer = json.parse(@"
	{
		""name"": null,
		""surname"": null,
		""addresses"": []
	}");

/*
	* {
	*   "name": "Muhammet",
	*   "surname": null,
	*   "addresses": []
	* }
	*/
developer.name = "Muhammet";
/*
	* {
	*   "name": "Muhammet",
	*   "surname": "Kandemir",
	*   "addresses": []
	* }
	*/
developer.surname = "Kandemir";
/*
	* {
	*   "name": "Muhammet",
	*   "surname": "Kandemir",
	*   "className": "C1",
	*   "addresses": []
	* }
	*/
developer.className = "C1";
/*
	* {
	*   "name": "Muhammet",
	*   "surname": "Kandemir",
	*   "className": "C1",
	*   "addresses": [
	*      "Address 1",
	*      "Address 2"
	*   ]
	* }
	*/
developer.addresses = new object[]
	{
		"Address 1",
		"Address 2"
	};
/*
	* {
	*   "name": "Muhammet",
	*   "surname": "Kandemir",
	*   "className": "Kandemir",
	*   "addresses": [
	*      "Address 1",
	*      "Address 2",
	*      "Address 3"
	*   ]
	* }
	*/
developer.addresses.push("Address 3");
 ```
 
 ## Example 2
 
  We can also use download.
 
 ```csharp
/*
	* https://muhammet-kandemir-95.github.io/dmuka3.JS.Simple.OCD/examples/example-data.json
	* {
	*   "name": "Muhammet",
	*   "surname": "Kandemir"
	* }
	*/
var developer = json.download("https://muhammet-kandemir-95.github.io/dmuka3.JS.Simple.OCD/examples/example-data.json").data;

/*
	* {
	*   "name": "Muhammet",
	*   "surname": "Kandemir",
	*   "className": "Kandemir"
	* }
	*/
developer.className = "C1";

/*
	* {
	*   "name": "Muhammet",
	*   "surname": "Kandemir",
	*   "className": "C1",
	*   "addresses": [
	*      "Address 1",
	*      "Address 2"
	*   ]
	* }
	*/
developer.addresses = new object[]
{
	"Address 1",
	"Address 2"
};

/*
	* {
	*   "name": "Muhammet",
	*   "surname": "Kandemir",
	*   "className": "C1",
	*   "addresses": [
	*      "Address 1",
	*      "Address 2",
	*      "Address 3"
	*   ]
	* }
	*/
developer.addresses.push("Address 3");
 ```
