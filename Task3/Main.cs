using Task3;

Dictionary<string, double> products = new Dictionary<string, double>{
    { "Bananas", 13.3f },
    { "Chocolate", 18},
    { "Fish", 2},
    { "Meat", -15},
    { "IceCream", -18},
    { "FrozenPizza", -30},
    { "Cheese", 7.2},
    { "Sausages", 5},
    { "Butter", 20.5},
    { "Eggs", 19}
};
// With further reflection, yeah this should have been a dictionary
List<Container> containers = new List<Container>();
List<ContainerShip> containerShips = new List<ContainerShip>();
void CreateContainerShip(List<ContainerShip> containerShips)
    {
        Console.WriteLine("Max Speed (in knots)? ");
        double maxSpeed = double.Parse(Console.ReadLine() ?? "0");
        Console.WriteLine("Max Container Count? ");
        int containerCount = int.Parse(Console.ReadLine() ?? "1");
        Console.WriteLine("Max Carrying Capacity (in tons)? ");
        double maxCarryingCapacity = double.Parse(Console.ReadLine() ?? "0");
        containerShips.Add(new ContainerShip(maxSpeed, containerCount, maxCarryingCapacity));
    }
    void CreateContainer(List<Container> containerList, Dictionary<string, double> products)
    {
        try
        {
            Console.WriteLine("Which container do you want to create?\n1. Liquid\n2. Gas\n3. Chilled");
            string? answer = Console.ReadLine();
            if (string.IsNullOrEmpty(answer)) return;
            Console.Write("Height (in cm)? ");
            double height = double.Parse(Console.ReadLine());
            Console.Write("Weight (in kg)? ");
            double weight = double.Parse(Console.ReadLine());
            Console.Write("Depth (in cm)? ");
            double depth = double.Parse(Console.ReadLine());
            Console.Write("Maximum payload (in kg)? ");
            double maximumPayload = double.Parse(Console.ReadLine());
            switch (answer.Substring(0, 1))
            {
                case "1":
                    containerList.Add(new LiquidContainer(height, weight, depth, maximumPayload));
                    break;
                case "2":
                    Console.Write("Atmosphere (in Pa)? ");
                    double atmosphere = double.Parse(Console.ReadLine());
                    containerList.Add(new GasContainer(height, weight, depth, maximumPayload, atmosphere));
                    break;
                case "3":
                    Console.Write("Product name? ");
                    string productName = Console.ReadLine() ?? "";
                    if (string.IsNullOrEmpty(productName))
                    {
                        Console.WriteLine("Product not found");
                        return;
                    }

                    Console.Write("Temperature (in \u00b0C)? ");
                    double temperature = double.Parse(Console.ReadLine());
                    containerList.Add(new ChilledContainer(height, weight, depth, maximumPayload, temperature,
                        productName, products));
                    break;
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
        }
    }

    Container? GetContainerFromList(List<Container> containerList)
    {
        try
        {
            Console.WriteLine("Number series of the container you want to load ");
            foreach (Container container in containerList)
            {
                Console.WriteLine(container.ToString());
            }

            string number = Console.ReadLine();
            foreach (Container container in containerList)
            {
                if (container.SeriesNumber.ToUpper().Equals(number.ToUpper()))
                {
                    return container;
                }
            }
        }
        catch (Exception e)
        {
            return null;
        }
        return null;
    }
    void LoadContainer(List<Container> containerList)
    {
        Container? container = GetContainerFromList(containerList);
        if (container == null) return;
        Console.Write("Payload weight (in kg)? ");
        double payloadWeight = double.Parse(Console.ReadLine() ?? "0");
        try
        {
            if (container.ContainerType == "L")
            {
                Console.Write("Is the payload Dangerous (y/n) ?");
                string answer = Console.ReadLine() ?? "";
                if (string.IsNullOrEmpty(answer)) return;
                if (answer.ToLower() == "y")
                {
                    ((LiquidContainer)container).Load(payloadWeight, LiquidPayloadType.Dangerous);
                }
                else ((LiquidContainer)container).Load(payloadWeight, LiquidPayloadType.Safe);
            }
            else
            {
                container.Load(payloadWeight);
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
        }
    }

    ContainerShip? ChooseContainerShip(List<ContainerShip> shipList)
    {
        int counter = 0;
        Console.WriteLine("Choose the number of the ship you want: ");
        foreach (ContainerShip containerShip in shipList)
        {
            Console.WriteLine($"{counter++}. {containerShip.ToString()}");
        }
        int? answer = int.Parse(Console.ReadLine() ?? "0");
        if (answer.Value >= 0 && answer < shipList.Count)
        {
            return shipList.ElementAt(answer.Value);
        }
        return null;
    }

    Container? ChooseContainer(List<Container> containerList)
    {
        Console.WriteLine("Choose the series number of the container you want: ");
        foreach (Container container in containerList)
        {
            Console.WriteLine($"{container.SeriesNumber}");
        }
        string answer = Console.ReadLine() ?? "";
        foreach (Container container in containerList)
        {
            if (container.SeriesNumber.ToUpper().Equals(answer.ToUpper()))
                return container;
        }
        return null;
    }

    void LoadContainerShip(List<ContainerShip> shipList, List<Container> containerList)
    {
        ContainerShip? cs = ChooseContainerShip(shipList);
        if (cs == null) return;
        Container? c = ChooseContainer(containerList);
        if (c == null) return;
        containerList.Remove(c);
        try
        {
            cs.AddContainer(c);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            containerList.Add(c);
        }
    }

    void LoadContainersOntoShip(List<ContainerShip> shipList, List<Container> containerList)
    {
        // Okay seriously I have no idea how you would keep track of objects loaded onto the ship
        // for my sanity please let's just assume you don't want to destroy the original list
        ContainerShip? cs = ChooseContainerShip(shipList);
        if (cs == null) return;
        foreach (Container c in containerList)
        {
            try
            {
                cs.AddContainer(c);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                break;
            }
        }
    }

    Container? RemoveContainerFromShip(List<ContainerShip> shipList, List<Container> containerList)
    {
        ContainerShip? cs = ChooseContainerShip(shipList);
        if (cs == null) return null;
        Console.WriteLine("Series number of the container you want to remove: ");
        cs.ShowContainers();
        string number = Console.ReadLine() ?? "";
        return cs.RemoveContainer(number);
    }
    
    void RemoveContainerFromList(List<Container> containerList)
    {
        Console.WriteLine("Series number of the container you want to remove: ");
        foreach (Container container in containerList)
        {
            Console.WriteLine(container.ToString());
        }
        string number = Console.ReadLine() ?? "";
        foreach (Container container in containerList)
        {
            if (container.SeriesNumber.ToUpper().Equals(number.ToUpper()))
            {
                containerList.Remove(container);
                break;
            }
        }
    }

    void UnloadContainer(List<Container> containerList)
    {
        Console.WriteLine("Series number of the container you want to unload: ");
        foreach (Container container in containerList)
        {
            Console.WriteLine(container.ToString());
        }
        string number = Console.ReadLine() ?? "";
        foreach (Container container in containerList)
        {
            if (container.SeriesNumber.ToUpper().Equals(number.ToUpper()))
            {
                container.Unload();
                break;
            }
        }
    }

    // Hate, let me tell you how much I have begun to hate this method
    void ReplaceContainer(List<ContainerShip> shipList, List<Container> containerList)
    {
        ContainerShip? cs = ChooseContainerShip(shipList);
        if (cs == null) return;
        Console.WriteLine("Series number of the container you want to replace: ");
        cs.ShowContainers();
        string number = Console.ReadLine() ?? "";
        Container? c = cs.RemoveContainer(number);
        if (c == null) return;
        containerList.Add(c);
        Console.WriteLine("Series number of the container you want to add: ");
        foreach (Container container in containerList)
        {
            Console.WriteLine(container.ToString());
        }
        number = Console.ReadLine() ?? "";
        if (number == "") return;
        try
        {
            foreach (Container container in containerList)
            {
                if (container.SeriesNumber.ToUpper().Equals(number.ToUpper()))
                {
                    cs.AddContainer(container);
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
        }
    }

    void RemoveContainerShip(List<ContainerShip> shipList)
    {
        ContainerShip? cs = ChooseContainerShip(shipList);
        if (cs == null) return;
        shipList.Remove(cs);
    }
bool flag = false;
int loadedContainers = 0;
while (!flag)
{
    Console.Clear();
    Console.WriteLine("ContainerShip list:");
    int counter = 0;
    if (containerShips.Count > 0)
    {
        foreach (ContainerShip containerShip in containerShips)
        {
            Console.WriteLine($"{++counter}.{containerShip.ToString()}");
            loadedContainers += containerShip._containers.Count;
        }
    }
    else Console.WriteLine("No container ships yet.");
    counter = 0;
    Console.WriteLine("Containers list:");
    if (containers.Count > 0)
    {
        foreach (Container container in containers)
        {
            Console.WriteLine($"{++counter}.{container.ToString()}");
        }
    }
    else Console.WriteLine("No containers yet.");
    Console.WriteLine("A) Add a container ship.");
    if (containerShips.Count > 0) Console.WriteLine("B) Remove a container ship.");
    Console.WriteLine("C) Add a container.");
    if (containers.Count > 0) Console.WriteLine("D) Remove a container.");
    if (loadedContainers > 0) Console.WriteLine("E) Remove a container from a ship.");
    if (containers.Count > 0) Console.WriteLine("F) Unload a container.");
    if (containers.Count > 0 && containerShips.Count > 0) Console.WriteLine("G) Load a container onto a ship.");
    if (containers.Count > 0) Console.WriteLine("H) Load a container.");
    if (containerShips.Count > 0) Console.WriteLine("I) Get ship information.");
    Console.WriteLine("J) Exit the program.");
    string input = Console.ReadLine() ?? "";
    switch (input.ToUpper())
    {
        case "A":
            CreateContainerShip(containerShips);
            break;
        case "B":
            RemoveContainerShip(containerShips);
            break;
        case "C":
            CreateContainer(containers, products);
            break;
        case "D":
            RemoveContainerFromList(containers);
            break;
        case "E":
            Container? c = RemoveContainerFromShip(containerShips, containers);
            if (c != null) containers.Add(c);
            break;
        case "F":
            UnloadContainer(containers);
            break;
        case "G":
            LoadContainerShip(containerShips, containers);
            break;
        case "H":
            LoadContainer(containers);
            break;
        case "I":
            ContainerShip? cs = ChooseContainerShip(containerShips);
            if (cs == null) break;
            Console.WriteLine(cs.ToString());
            cs.ShowContainers();
            Thread.Sleep(3000);
            break;
        case "J":
            flag = true;
            Console.WriteLine("Program shutting down.");
            Thread.Sleep(1000);
            break;
    }
    Thread.Sleep(500);
    Console.Clear();
}


