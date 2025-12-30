#!/bin/bash

# Colors
WHITE_BG='\033[47m'
RED_BG='\033[41m'
GREEN='\033[0;32m'
RED='\033[0;31m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
WHITE='\033[1;37m'
GRAY='\033[0;90m'
BLACK='\033[0;30m'
NC='\033[0m'
BOLD='\033[1m'

# Icons
CHECK=""
CROSS=""
PACKAGE=""
TEST=""
COV=""

clear

echo ""
echo -e "${CYAN}╔═══════════════════════════════════════════════════════════╗${NC}"
echo -e "${CYAN}║${NC}  ${TEST}  ${WHITE}PeerDrop Backend - Unit Test Suite${NC}                ${CYAN}     ║${NC}"
echo -e "${CYAN}╚═══════════════════════════════════════════════════════════╝${NC}"
echo ""

# Delete old test results
rm -rf ./TestResults 2>/dev/null

echo -e "${GRAY}${PACKAGE} Building projects...${NC}"

# Run tests with TRX output
dotnet test \
  --logger "trx;LogFileName=test-results.trx" \
  --logger "console;verbosity=quiet" \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults \
  --nologo \
  /p:CollectCoverage=true > /dev/null 2>&1

TEST_EXIT_CODE=$?

echo ""
echo -e "${CYAN}═══════════════════════════════════════════════════════════${NC}"
echo -e "  ${TEST} ${WHITE}Test Results${NC}"
echo -e "${CYAN}═══════════════════════════════════════════════════════════${NC}"
echo ""

# Parse TRX file
TRX_FILE=$(find ./TestResults -name "test-results.trx" | head -1)

if [ -f "$TRX_FILE" ]; then
    # Extract test results
    TOTAL=$(grep -o 'total="[0-9]*"' "$TRX_FILE" | sed 's/total="\([0-9]*\)"/\1/')
    PASSED=$(grep -o 'passed="[0-9]*"' "$TRX_FILE" | sed 's/passed="\([0-9]*\)"/\1/')
    FAILED=$(grep -o 'failed="[0-9]*"' "$TRX_FILE" | sed 's/failed="\([0-9]*\)"/\1/')
    
    # Parse individual tests
    echo -e "${WHITE}Individual Test Results:${NC}"
    echo ""
    
    # Parse test names and outcomes
    grep -o '<UnitTestResult.*testName="[^"]*".*outcome="[^"]*"' "$TRX_FILE" | while read -r line; do
        TEST_NAME=$(echo "$line" | sed 's/.*testName="\([^"]*\)".*/\1/')
        OUTCOME=$(echo "$line" | sed 's/.*outcome="\([^"]*\)".*/\1/')
        
        # Shorten test name for display
        SHORT_NAME=$(echo "$TEST_NAME" | sed 's/PeerDrop.Tests.Services.//' | sed 's/Tests\.//')
        
        if [ "$OUTCOME" = "Passed" ]; then
            # Pytest-style PASSED badge - white background, green text
            echo -e "  ${WHITE_BG}${GREEN}${BOLD} PASSED ${NC} ${GRAY}${SHORT_NAME}${NC}"
        else
            # Red FAILED badge
            echo -e "  ${RED_BG}${WHITE}${BOLD} FAILED ${NC} ${GRAY}${SHORT_NAME}${NC}"
        fi
    done
    
    echo ""
    echo -e "${CYAN}═══════════════════════════════════════════════════════════${NC}"
    echo ""
    
    # Summary box
    if [ "$FAILED" = "0" ]; then
        echo -e "  ${GREEN}╔═══════════════════════════════════════════════════╗${NC}"
        echo -e "  ${GREEN}║${NC}  ${WHITE_BG}${GREEN}${BOLD} ✓ ALL TESTS PASSED ${NC}                       ${GREEN}      ║${NC}"
        echo -e "  ${GREEN}╠═══════════════════════════════════════════════════╣${NC}"
        echo -e "  ${GREEN}║${NC}  ${WHITE}Tests:${NC}    ${GREEN}${TOTAL} passed${NC}, ${WHITE}${TOTAL} total${NC}                    ${GREEN}║${NC}"
        echo -e "  ${GREEN}║${NC}  ${WHITE}Status:${NC}   ${WHITE_BG}${GREEN}${BOLD}  SUCCESS ${NC}                             ${GREEN}║${NC}"
        echo -e "  ${GREEN}╚═══════════════════════════════════════════════════╝${NC}"
    else
        echo -e "  ${RED}╔═══════════════════════════════════════════════════╗${NC}"
        echo -e "  ${RED}║${NC}  ${RED_BG}${WHITE}${BOLD} ✗ TESTS FAILED ${NC}                             ${RED}║${NC}"
        echo -e "  ${RED}╠═══════════════════════════════════════════════════╣${NC}"
        echo -e "  ${RED}║${NC}  ${WHITE}Tests:${NC}    ${GREEN}${PASSED} passed${NC}, ${RED}${FAILED} failed${NC}, ${WHITE}${TOTAL} total${NC}      ${RED}║${NC}"
        echo -e "  ${RED}║${NC}  ${WHITE}Status:${NC}   ${RED_BG}${WHITE}${BOLD} FAILED ${NC}                             ${RED}║${NC}"
        echo -e "  ${RED}╚═══════════════════════════════════════════════════╝${NC}"
    fi
    
    echo ""
    
    # Generate coverage report
    COVERAGE_FILE=$(find ./TestResults -name "coverage.cobertura.xml" | head -1)
    if [ -n "$COVERAGE_FILE" ]; then
        echo -e "${GRAY}${COV} Generating coverage report...${NC}"
        reportgenerator \
          -reports:./TestResults/**/coverage.cobertura.xml \
          -targetdir:./TestResults/CoverageReport \
          -reporttypes:Html 2>/dev/null
        
        if [ $? -eq 0 ]; then
            echo -e "${GREEN}${CHECK}${NC} Coverage report: ${CYAN}./TestResults/CoverageReport/index.html${NC}"
            
            # Try to extract coverage percentage
            if command -v xmllint &> /dev/null; then
                LINE_RATE=$(xmllint --xpath 'string(//coverage/@line-rate)' "$COVERAGE_FILE" 2>/dev/null)
                if [ -n "$LINE_RATE" ]; then
                    COVERAGE_PCT=$(echo "scale=1; $LINE_RATE * 100" | bc 2>/dev/null)
                    if [ -n "$COVERAGE_PCT" ]; then
                        echo -e "${GREEN}${CHECK}${NC} Code Coverage: ${GREEN}${COVERAGE_PCT}%${NC}"
                    fi
                fi
            fi
        fi
    fi
    
    echo ""
    
    if [ "$FAILED" != "0" ]; then
        exit 1
    fi
else
    echo -e "${RED}${CROSS} Test results file not found!${NC}"
    exit 1
fi