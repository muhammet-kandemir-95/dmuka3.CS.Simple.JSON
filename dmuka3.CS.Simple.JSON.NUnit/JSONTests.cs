using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using dmuka3.CS.Simple.JSON;

namespace dmuka3.CS.Simple.JSON.NUnit
{
    public class JSONTests
    {
        [Test]
        public void JObjectTest()
        {
            var obj = new
            {
                testprop1 = 123,
                testprop2 = new
                {
                    testprop3 = 456,
                    testprop4 = 789
                }
            };

            var objJSON = JsonConvert.SerializeObject(obj);

            dynamic objDeserialize = json.parse(objJSON);

            Assert.AreEqual(obj.testprop1, objDeserialize.testprop1.v);
            Assert.AreEqual(obj.testprop2.testprop3, objDeserialize.testprop2.testprop3.v);
            Assert.AreEqual(obj.testprop2.testprop4, objDeserialize.testprop2.testprop4.v);
        }

        [Test]
        public void JArrayTest()
        {
            var obj = new object[]
            {
                123,
                "abc",
                456,
                "dfe",
                789
            };

            var objJSON = JsonConvert.SerializeObject(obj);

            dynamic objDeserialize = json.parse(objJSON);

            Assert.AreEqual(obj[0], objDeserialize[0].v);
            Assert.AreEqual(obj[1], objDeserialize[1].v);
            Assert.AreEqual(obj[2], objDeserialize[2].v);
            Assert.AreEqual(obj[3], objDeserialize[3].v);
            Assert.AreEqual(obj[4], objDeserialize[4].v);
        }

        [Test]
        public void DynamicTest()
        {
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

            Assert.AreEqual(developer.ToString(), "{\"name\":\"Muhammet\",\"surname\":\"Kandemir\",\"addresses\":[\"Address 1\",\"Address 2\",\"Address 3\"],\"className\":\"C1\"}");
        }

        [Test]
        public void NetworkTest()
        {
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

            Assert.AreEqual(developer.ToString(), "{\"name\":\"Muhammet\",\"surname\":\"Kandemir\",\"className\":\"C1\",\"addresses\":[\"Address 1\",\"Address 2\",\"Address 3\"]}");
        }
    }
}