using CameraDiplomat.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraDiplomat.Services
{
	public class MessageDecoder
	{
		public Product MessegeDiplomat(string messageToDiplomating)
		{
			//message must to have a json format or <start>Quality<is>false3<percent>50<codeIs>123456789abcdef<textIs>none<end>
			// *start*Quality*is*false*percent*50*codeIs*123456789abcdef*textIs*none*end*
			try
			{
				string[] messages;
				//messages = messageToDiplomating.Split('*');
				messages = messageToDiplomating.Split('<', '>');

				if (messages.Count() == 13)
				{
					Product newProduct = new Product();
					newProduct.data = DateTime.Now.ToString();
					newProduct.Id = Guid.NewGuid().ToString();
					newProduct.productName = messages[2];
					newProduct.quality = Boolean.Parse(messages[4]);
					newProduct.percent = Int32.Parse(messages[6]);
					newProduct.code = messages[8];
					newProduct.text = messages[10];

					return newProduct;
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
