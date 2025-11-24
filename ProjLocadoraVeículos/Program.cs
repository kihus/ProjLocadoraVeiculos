using Locadora.View;

var menu = new MenuPrincipal();
try
{
    await menu.Menu();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}
