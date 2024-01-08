// See https://aka.ms/new-console-template for more information
/* Lim Chang Ze - S10255850
 * Elvis Chan Jun Yu - S10259428
 * Assignment 1 */

using Assignment1_ChangZe_Elvis;

Console.Write("Enter option: ");
string option = Console.ReadLine();
Console.Write("Enter scoops: ");
int scoops = Convert.ToInt32(Console.ReadLine());

IceCream icecream = new Cup(option, scoops);
Console.WriteLine(icecream.ToString());