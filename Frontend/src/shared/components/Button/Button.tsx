import { type ButtonHTMLAttributes, type ReactNode } from "react";
import clsx from "clsx";

type ButtonProps =
    | ( {
        variant?: "primary" | "secondary" | "outline";
        size?: "sm" | "md" | "lg";
        icon?: ReactNode;
        children?: ReactNode;
    } & ButtonHTMLAttributes<HTMLButtonElement> )
    | ( {
        variant: "icon";
        icon: ReactNode;
        size?: never;
        children?: never;
    } & ButtonHTMLAttributes<HTMLButtonElement> );

export default function Button ( {
    variant = "primary",
    size = "md",
    children,
    icon,
    className,
    ...props
}: ButtonProps ) {
    const baseStyles =
        "group relative inline-flex items-center justify-center font-bold uppercase tracking-widest border-4 border-black transition-all active-press";

    const variants = {
        primary: "bg-primary text-black shadow-brutal hover:bg-white",
        secondary:
            "bg-white text-black shadow-brutal-sm hover:bg-secondary hover:text-white hover:shadow-brutal",
        outline:
            "bg-transparent text-black border-2 border-black hover:bg-black hover:text-white",
        icon:
            "w-10 h-10 border-2 border-white bg-black text-white hover:bg-primary hover:text-black hover:border-black",
    };

    const sizes = {
        sm: "px-4 py-2 text-xs",
        md: "px-6 py-3 text-sm",
        lg: "px-8 py-5 text-xl w-full h-16",
        icon: "p-0",
    };

    const finalClassName = clsx(
        baseStyles,
        variants[ variant ],
        variant === "icon" ? sizes.icon : sizes[ size ],
        className
    );

    return (
        <button className={ finalClassName } { ...props }>
            <span className="flex items-center gap-2">
                { children }
                { icon && (
                    <span className="transition-transform group-hover:translate-x-1">
                        { icon }
                    </span>
                ) }
            </span>
        </button>
    );
}
