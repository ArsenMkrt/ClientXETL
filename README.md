# ClientXETL - ETL Use Case Challenge
🚀 Purpose
This project demonstrates a small ETL use case to:

Understand coding style and problem-solving skills.
Showcase how the team works daily through a simplified project fragment.
Provide code for discussions in an interview.
📋 Context
This project focuses on a simplified ETL (Extract, Transform, Load) workflow, typically used to manage large datasets. For the purpose of this challenge, the ETL process is reduced to:

Step 1: Unzip Files
Extract ClientX_Interface.zip containing POLICY.txt and RISK.txt.
Step 2: Validate Data
Ensure data quality with structural and business rule validations.
Step 3: CRUD Modeling
Load the validated data into in-memory structures (mimics tables for CRUD operations).
🛠️ Features
File Unzipping and Parsing:

POLICY.txt: Contains policy data (ID, PolicyName).
RISK.txt: Contains risk data (ID, RiskName, Peril, PolicyID, Street, CID, Lat, Long).
Data Validation Rules:

Validates structure (e.g., required columns, correct data types).
Applies business rules (e.g., Peril must be one of the defined enum values, latitude/longitude constraints).
In-Memory CRUD Operations:

Use MemoryCache or equivalent structures to model the data.
Unit Testing:

3-4 unit tests designed to validate functionality, such as:
Successfully loading data.
Detecting invalid data.
Correct CRUD operations.
🔧 How to Use
Setup
Clone the repository:

bash


git clone https://github.com/username/ClientXETL.git
cd ClientXETL
Build the project:

bash


dotnet build
Run the project:

bash


dotnet run
Example Usage
Step 1: Unzip Files
The ClientXDataExtractorService handles unzipping and reading POLICY.txt and RISK.txt.

Step 2: Validate Data
Validators such as GreaterThanValidationRule, NotNullOrEmptyValidationRule, and RangeValidationRule ensure data quality.

Step 3: Load & CRUD Operations
Data is parsed and stored in in-memory structures like PolicyStorage and RiskStorage for easy CRUD operations.

🧪 Testing
Run unit tests to validate functionality: