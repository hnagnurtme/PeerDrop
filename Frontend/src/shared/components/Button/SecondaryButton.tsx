import type React from "react";
import Button from "./Button";


export default function SecondaryButton( props : React.ComponentProps<typeof Button>){
    return (
        <Button variant="secondary" {...props} />
    )
}