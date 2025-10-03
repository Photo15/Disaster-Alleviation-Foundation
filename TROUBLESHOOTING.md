# Incident Submission Troubleshooting Guide

## Issue: Incident reports not being submitted

The incident submission functionality has been analyzed and several potential issues have been identified and fixed.

## Changes Made:

### 1. Enhanced Controller Logging
- Added comprehensive logging to `IncidentReportsController.cs` to track form submissions
- Added ModelState validation error logging
- Added exception handling with detailed error messages

### 2. Improved Form Validation
- Added explicit `method="post"` to the form
- Added anti-forgery token explicitly with `@Html.AntiForgeryToken()`
- Added better error display in the Create view

### 3. Success Message Display
- Added TempData success message display in Index view
- Enhanced user feedback for successful submissions

## Debugging Steps:

### Step 1: Check Browser Console
1. Open the incident creation page
2. Open browser developer tools (F12)
3. Go to Console tab
4. Fill out and submit the form
5. Check for any JavaScript errors

### Step 2: Check Network Tab
1. In developer tools, go to Network tab
2. Submit the form
3. Look for the POST request to `/IncidentReports/Create`
4. Check if the request is being sent and what the response is

### Step 3: Check Application Logs
When running the application, check the console output for log messages like:
- "Create POST action called"
- "Model received - Title: ..."
- "ModelState Error - Key: ..."
- "Adding incident to context: ..."
- "SaveChangesAsync returned: X changes"

## Common Issues and Solutions:

### Issue 1: .NET SDK Not Installed
**Symptom:** `dotnet` command not recognized
**Solution:** Install .NET 8.0 SDK from https://dotnet.microsoft.com/download/dotnet/8.0

### Issue 2: Database Not Created
**Symptom:** Database-related errors
**Solution:** Run `dotnet ef database update` to apply migrations

### Issue 3: User Not Authenticated
**Symptom:** Form submits but no user ID
**Solution:** Ensure user is logged in before submitting incidents

### Issue 4: Model Validation Errors
**Symptom:** Form doesn't submit, no redirect occurs
**Solution:** Check browser console and application logs for validation errors

## How to Run the Application:

### Method 1: Using Batch File (Recommended)
1. Double-click `run-app.bat` in the project folder
2. The script will check for .NET SDK and run the application

### Method 2: Manual Commands
1. Open PowerShell or Command Prompt
2. Navigate to project folder: `cd "c:\Users\pc\Desktop\Photo"`
3. Apply migrations: `dotnet ef database update`
4. Run application: `dotnet run`

## Testing the Fix:

1. Start the application using one of the methods above
2. Navigate to the application URL (usually https://localhost:5001)
3. Register a new user or log in
4. Click on "Incident Reports" in the navigation
5. Click "Report New Incident"
6. Fill out all required fields:
   - Title (required)
   - Description (required)
   - Location (required)
   - Priority (defaults to Medium)
7. Click "Submit Report"
8. You should be redirected to the incident list page
9. A success message should appear at the top
10. Your incident should appear in the list

## Files Modified:

1. `Controllers/IncidentReportsController.cs` - Enhanced logging and error handling
2. `Views/IncidentReports/Create.cshtml` - Added anti-forgery token
3. `Views/IncidentReports/Index.cshtml` - Added success message display
4. `run-app.bat` - Created application runner script
5. `test-form.html` - Created test form for debugging

## Database Schema:

The IncidentReport model includes:
- Id (auto-generated)
- Title (required, max 200 characters)
- Description (required, max 2000 characters)
- Location (required, max 500 characters)
- DateReported (auto-set to current datetime)
- ReportedByUserId (auto-set to current user)
- Status (defaults to "Open")
- Priority (user-selected: Low/Medium/High)

All fields are properly validated and the database will be created automatically when the application runs for the first time.