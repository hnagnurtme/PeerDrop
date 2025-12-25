# src/

Root source directory.

## Structure
- app/       App bootstrap & global config
- features/  Business features (domain-driven)
- shared/    Reusable, app-agnostic code
- assets/    Static assets
- styles/    Global styles

## Rules
- Business logic must live in `features/`
- `app/` must stay thin
