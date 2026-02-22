#######################################################
# AI AGENT ORCHESTRATOR FOR UNITY (Windows/PowerShell)
#######################################################

# --- 1. CONFIGURATION ---
$UNITY_PATH  = "C:\Program Files\Unity\Hub\Editor\2022.3.0f1\Editor\Unity.exe" # <-- ADJUST IF NEEDED
$PROJECT_PATH = Get-Location
$LOG_FILE     = "$PROJECT_PATH\build.log"
$AIDER_PATH   = "C:\Users\Admin\.local\bin\aider.exe"
$STOP_FILE    = "$PROJECT_PATH\STOP_AGENT"

# --- 2. SAFETY LIMITS ---
$MAX_ITERATIONS = 80    # The agent will stop after this many tasks
$SLEEP_SECONDS  = 5    # Time between tasks
$iterationCount = 0

# --- 3. PRE-FLIGHT CHECKS ---
if (-not (Test-Path $AIDER_PATH)) {
    Write-Host "ERROR: Aider not found at $AIDER_PATH" -ForegroundColor Red
    Write-Host "Please run: python -m venv .venv; .\.venv\Scripts\pip install aider-chat"
    exit
}

if (-not (Test-Path "TODO.md")) {
    Write-Host "ERROR: TODO.md not found in root folder!" -ForegroundColor Red
    exit
}

# --- 4. MAIN LOOP ---
Write-Host "Pirate Agent is starting..." -ForegroundColor Green

while ($iterationCount -lt $MAX_ITERATIONS) {
    
    # Check for manual stop file
    if (Test-Path $STOP_FILE) {
        Write-Host "Manual stop file detected ($STOP_FILE). Exiting loop..." -ForegroundColor Yellow
        break
    }

    $iterationCount++
    $time = Get-Date -Format "HH:mm:ss"
    Write-Host "`n[$time] --- ITERATION $iterationCount OF $MAX_ITERATIONS ---" -ForegroundColor Cyan

    # --- 5. EXECUTE AIDER ---
    # We use '&' (Call Operator) to run the full path to the .venv executable
    & $AIDER_PATH --yes-always --architect --message "
        1. Read TODO.md and find the first task marked with [ ].
        2. Implement it using C# in Unity. 
        3. Follow these rules: Namespace 'PirateGame.Core', use [SerializeField], comments in English.
        4. After coding, verify the build by running this command exactly:
           '$UNITY_PATH -batchmode -projectPath $PROJECT_PATH -quit -logFile $LOG_FILE'
        5. If build.log contains 'error', fix the code until it compiles.
        6. Once build is clean, mark task as [x] in TODO.md and commit with a clear message.
    "

    # Check if we hit the limit
    if ($iterationCount -ge $MAX_ITERATIONS) {
        Write-Host "Reached maximum iterations ($MAX_ITERATIONS). Stopping for review." -ForegroundColor Red
        break
    }

    Write-Host "Task complete. Waiting $SLEEP_SECONDS seconds before next task..." -ForegroundColor Gray
    Start-Sleep -Seconds $SLEEP_SECONDS
}

Write-Host "`nAgent session finished. Check your Git history and TODO.md." -ForegroundColor Green

shutdown /s /f /t 60