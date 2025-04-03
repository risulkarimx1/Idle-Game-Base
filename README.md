# Idle Game Framework

## Solution objective:

The main objective of the proposed solution is to effectively deliver a base game for idle games in general. In addition, I have taken the initiative to investigate potential avenues for developing a production-ready, scalable project. This approach ensures that the project remains modular, facilitating the easy addition of further resources and enabling seamless continuation of its development.

# Key features 💡

1. Make the system robust and highly testable through **Dependency Inversion**, adhering to SOLID principles wherever possible.
2. Properly create a **BootScene** and other scenes, defining their responsibilities clearly.
3. Utilize **Zenject** for easier dependency resolution and context distribution across the project, enhancing maintainability and scalability.
4. Employ **UniTask** for asynchronous operations, ensuring zero memory allocation while operating on Unity’s main thread, to guarantee smooth loading, unloading, and a non-blocking user experience.
5. Implement an **Addressable**-based loading and unloading feature for scene objects to manage memory efficiently.
6. Develop robust and reusable frameworks, such as **DataManager** and **GameInitManager**, LogFramework, etc., to speed up project scalability without extensive boilerplate code.
7. Incorporate unit testing to mitigate potential calculation errors and oversights. Use **Zenject test fixtures** and a **Moq-based** approach for extendability.
8. Facilitate decoupled service communication through a zenject **signal-based** approach.
9. **Odin Inspector**-based **configurations** editor for Quality Of Life features.
10. Overall aim for high standards in scalability by developing reusable frameworks and services with product-level practices.

## Project architecture:

Based on the given solution, with single scene `GameScene`,  game config as `GameConfig`, and entry point as `GameInitializer`, I made further modifications to ease the extension. The whole process can be described as the following points.

1. **DiContainer: Zenject** as a DiContainer is introduced to resolve dependencies at a particular step.
2. **Scene Breakdown:** Since there are multiple levels, a proper scene breakdown was introduced to initialize the game for the first time, then flow into the loading phase, the to-game phase. Along with these smooth transitions and loading and unloading of resources were ensured.
3. **Additional frameworks and services:** To ease the process of creating data files for persistence, and to ensure all the essential services are initialized several services were created. ****

## Scene and Di Context Orchestration:

The flow of the project is orchestrated by how Zenject Installers are created and Placed at Which Scene. The architecture of the project can be shown in the following diagram.

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/0794596d-6a99-42b1-8353-3509ad04b749/5cdebdf6-0efe-4fbc-8efd-0146a0e21384/Untitled.png)

## Data Management:

The project has a JSON file system-based Data Manager (details in the Frameworks section) which contains several files to load, update, and save the data on demand. The key fields these data files load are

1. **Finance Data:**
    1. Money and Deposit rate 
2. **Game Session Data:**
    1. Session LogOff time: To calculate the passive income
    2. MineID of the current game session
3. **Mines Data**
    1. Levels of Elevator and Warehouse
    2. Collection of MineShafts and their levels

## Game Config:

I extended the GameConfig with a few additional configurations. 

Ideally, such config files should remain in the cloud with a dashboard and API to access them. 

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/0794596d-6a99-42b1-8353-3509ad04b749/14cf00d7-1608-4a34-835a-8e6e23147c93/Untitled.png)

1. **DataEncryption** Section is used to encrypt and rotate the keys for security.
2. **Scenes** are also part of configs to make it decoupled from scene logic from scene files.
3. **MinesConfig:** The newly created mines configs contain information on new mines, their addressable assets, names, descriptions, etc. 
    1. The config ensures data binding with MinesData where MineId acts as a primary key. 
    2. It also provides essential information to show MineSelectionView via MineSelectionController. 

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/0794596d-6a99-42b1-8353-3509ad04b749/02b1ab4e-8c2a-4d41-bd88-89adfc68e058/Untitled.png)

# Game Life Cycle

## Step1: Entry Point: `ProjectContext`:

Location: `Assets/_Scripts/Installer/Resources/ProjectContext` 

As the entry point of the project, the ProjectContext ensures the loading following dependencies which are available across the project for all the scenes. Some of the key features it contains would be

1. **DataInstaller**: Responsible for loading persisting data files
2. **LoadingScreenInstaller**: Showing loading screen canvas during transitions
3. **ProjectContextInstaller**
    1. **GameConfig**: Instance of the `GameConfig` scriptable object.
    2. **Bootstrapper**: A relay service to move forward to the next scenes after initialization is complete.
    3. **GameInitManager**: Ensures the dependencies (e.g. `DataManger`) are loaded properly where they **are** needed.
    4. **GameSession** Services: To hold instances of the last played game sessions.

## Step 2: Boot Scene:

Ideally, the scene created assists Unity Engine to remain as a placeholder to load another scene additively. The scene acts as a relay toward `ScessionLoaderScene`. 
The only script on this scene `BootSceneManager` ensures that the dependencies are loaded (`DataManager` for the moment) and asynchronously loads `SessionLoaderScene.` 

## Step 3: SessionLoaderScene:

`IGameSessionProvider` provides `GameSession` object that connects with DataManager to get information about which Mine the `GameScene` should load. The object contains some other useful information such as asset keys for the **Addressable** which is going to be loaded in the GameScene. 

<aside>
💡 **SceneflowManager.SwitchScene** script is responsible to additively unload and load the Scenes. 
> **SessionLoaderScene** provides the Assets that need to be loaded.
> **SceneFlowManger**  then loads the GameScene Addively first, then unloads **SessionLoaderScene**

</aside>

## Step 4: Game Scene:

The reusable `GameScene` has the `GameInitializer`, `FinanceInitializer`, and script as the entry point. The script ensures that.

1. **MineShaft Creation:** MineShafts are created based on the `MineId` coming from `GameSession` along with their levels.
2. **Finance Model Setup:** `FinaceModel` is initiated with `the` game’s currency from `DataManager`>`FinanceData`. Along with data binding between `FinanceModel.Money` with `FinanceData.Money` .
3. **Passive Income Setup:** `PassiveIncome` is an optional feature that gives a bonus to the player when the player is away from the mine. Based on the away time of the player, and the income rate calculated from the `IdleIncomeCalculator` of a particular mine, the player receives the passive income bonus at the game’s initialization.
4. **MineSelection View and Controller:** The View has data binding with **MinesData** which provides information about the available mines, and their levels (elevator, shafts, and warehouse). The **MinesConfiguration** provides information such as name and description. The panel has a vertical scroll view to support a long list of mines.

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/0794596d-6a99-42b1-8353-3509ad04b749/af66fea1-1cc1-42b4-a26d-d81fa84285ad/Untitled.png)

When a new mine is selected

- `MineSelectionController.OnMineSelected(string mineId)` asynchronously closes the `MineSelectionView`
- Next, it commands `SceneFlowManager` to load `SessionLoaderScene`, under LoadingScreen.
- As the LoadingScreen appears, `SceneFlowManager` in the background unloads the addressable, clearing the memory.
- Then it loads **GameScene** and essential addressable to display the new mine.

<aside>
💡 As the game does not have a menu scene (not requested as part of the task), however placing a menu Scene is completely possible between the place of `BootScene` and `SessionLoadingScene`. `SceneFlowManager` is perfectly capable of carrying out such responsibility.

</aside>

# Frameworks and Services

Some general-purpose frameworks and services were necessary to develop for the project. Some of the key frameworks are

- DataManager
- GameInitManager
- LogFramework

## Data Manager:

- For the purpose of creating data files in object-oriented format with ease, a robust DataManager was made.
- The key idea behind the DataFramework is
    - To create a new Data Type with easy- Just by inhering `BaseData` class and assigning the attribute named `[DataIdentifier]`.
    - An interchangeable DataHandler, with the interface contract type `IDataHandler`. Currently, the project only has `JsonFileDataHandler`. But this handler can easily be switched with another format such as Binary or Cloud Storage (such as Firebase or Nakama).
    - On RunTime, changes in the data objects get loaded beginning from `DataManager.Initialize` method. After all the changes during the game, the data gets saved to the memory. All `Set` operations of data must have a `SetDirty()` function call to be marked for persistent save mechanism.
        - This ensures fast data manipulation without slowing down due to heavy operations such as Cloud Push or File IO.
    - While Switching game scenes, or closing the app, the `DataManger.SaveAllAsync` gets called to ensure persistence. The operation is asynchronous for a non-blocking smooth user experience.
    - The data is also secured with `IEncryptionService` can ensure data is encrypted with the `AesEncryption` algorithm via `AesEncryptionService`. The keys are stored in **GameConfig.** Rotating these keys can dynamically migrate the data into new files to ensure highly secured behavior along with robustness. The data file Migration is handled via `IDataHandler.LoadAsync` as the different handlers must implement the migrations in their own way.
    

## Game Init Manager

- GameInitManager is a class that manages the initialization of game components.
- It uses DiContainer for dependency injection to identify objects that require initialization of specific services such as DataManger. These services are marked with `IRequireInit` interface.
- The dependent classes are marked with `IInitializableAfter<IRequireInit>` or `IInitializableAfterAll` which ensure either dependencies are loaded and also deliver the dependent object as the parameter of `OnInitFinishedFor` method.

# Unit Tests

I added UnitTests to ensure each derived type of the **BaseData** behaves as expected. Common human errors, such as forgetting to call `SetDirty()` can easily be prevented via unit tests. 
Along with them I also added unit tests for PassiveIncomeCalculation to understand if the income is happening as per the calculation.

The tests are written with the NUnit framework along with `ZenjectTextFixture` and `Mock` classes. The overall architecture of the project is highly testable as it's done via Dependency Inversion and SRP in mind.

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/0794596d-6a99-42b1-8353-3509ad04b749/017c22c3-0e0b-45e4-bf42-c1e7f8dea48e/Untitled.png)

# Third-party tools

Odin inspector and Newtonsoft JSON Unity Converter package for serialization purposes.
