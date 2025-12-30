import  { type ButtonHTMLAttributes } from "react";
import clsx from "clsx";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement>{
    variant? : "primary" | "secondary"
}

export default function Button( 
    { children , variant = "primary" , className,  ...props} : ButtonProps
) {

    const baseClass = "mt-4 w-full h-16 bg-primary border-4 border-black shadow-brutal font-bold text-xl uppercase tracking-widest hover:bg-white active-press transition-all flex items-center justify-center gap-3 group no-underline text-black";
    const variantClass =
        variant === "primary"
        ? ""
        : "";

    return (
        <button className= {clsx(baseClass,variantClass, className)} {...props}>
            {children}
        </button>
    )
}
