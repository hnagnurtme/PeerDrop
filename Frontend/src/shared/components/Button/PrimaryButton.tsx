import type React from "react";
import Button from "./Button";


export default function PrimaryButton( props : React.ComponentProps<typeof Button>){
    return (
        <Button variant="primary" {...props} />
    )
}