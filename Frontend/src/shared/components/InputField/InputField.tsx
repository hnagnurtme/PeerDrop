import type { InputHTMLAttributes, ReactNode } from "react";
import clsx from "clsx";

interface InputFieldProps extends InputHTMLAttributes<HTMLInputElement> {
    label?: string;
    icon?: ReactNode;
    error?: string;
    helperText?: string;
}

export default function InputField ( {
    label,
    icon,
    error,
    helperText,
    className,
    id,
    ...props
}: InputFieldProps ) {
    const inputId = id ?? `input-${ props.name }`;

    const baseStyles =
        "w-full h-14 bg-terminal border-4 border-black px-4 font-mono text-lg font-bold placeholder:text-gray-300 focus:outline-none focus:bg-white focus:shadow-brutal-sm transition-all active:translate-y-[2px] active:translate-x-[2px]";

    const finalClassName = clsx(
        baseStyles,
        error && "border-secondary bg-secondary/5",
        className
    );

    return (
        <div className="flex flex-col gap-2 w-full">
            { ( label || icon ) && (
                <div className="flex justify-between items-end px-1">
                    { label && (
                        <label
                            htmlFor={ inputId }
                            className="text-[10px] font-bold uppercase tracking-[0.2em]"
                        >
                            { label }
                        </label>
                    ) }
                    { icon && <span className="text-sm opacity-60">{ icon }</span> }
                </div>
            ) }

            <div className="relative group">
                <input id={ inputId } { ...props } className={ finalClassName } />

                <div className="absolute -bottom-1 -right-1 w-2 h-2 bg-black opacity-0 group-focus-within:opacity-100 transition-opacity" />
            </div>

            { ( error || helperText ) && (
                <p
                    className={ clsx(
                        "text-[9px] font-bold uppercase tracking-wider px-1",
                        error ? "text-secondary" : "opacity-40"
                    ) }
                >
                    { error ? `Error: ${ error }` : `// ${ helperText }` }
                </p>
            ) }
        </div>
    );
}
