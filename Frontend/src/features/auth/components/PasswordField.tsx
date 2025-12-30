import clsx from "clsx";
import { useState, type InputHTMLAttributes } from "react";

type PasswordInputFieldProps =
    Omit<InputHTMLAttributes<HTMLInputElement>, "type"> & {
        label?: string;
        error?: string;
    };

export default function PasswordInputField ( {
    label,
    error,
    className,
    id,
    ...props
}: PasswordInputFieldProps ) {
    const [ isVisible, setIsVisible ] = useState( false );
    const inputId = id ?? `password-${ props.name }`;

    const baseStyles =
        "w-full h-14 bg-terminal border-4 border-black pl-4 pr-14 font-mono text-lg font-bold placeholder:text-gray-300 focus:outline-none focus:bg-white focus:shadow-brutal-sm transition-all";

    const finalClassName = clsx(
        baseStyles,
        error && "border-secondary bg-secondary/5",
        className
    );

    return (
        <div className="flex flex-col gap-2 w-full">
            { label && (
                <div className="flex justify-between items-end px-1">
                    { 
                        <label
                            htmlFor={ inputId }
                            className="text-[10px] font-bold uppercase tracking-[0.2em]"
                        >
                            { label }
                        </label>
                    }
                    <span
                        aria-hidden
                        className="material-symbols-outlined text-sm opacity-60"
                    >
                        lock
                    </span>
                </div>
            ) }

            <div className="relative group">
                <input
                    id={ inputId }
                    { ...props }
                    type={ isVisible ? "text" : "password" }
                    className={ finalClassName }
                />

                <button
                    type="button"
                    aria-label={ isVisible ? "Hide password" : "Show password" }
                    onClick={ () => setIsVisible( ( v ) => !v ) }
                    className="absolute right-0 top-0 h-full w-12 flex items-center justify-center border-l-4 border-black hover:bg-black hover:text-primary transition-colors focus:outline-none"
                >
                    <span className="material-symbols-outlined">
                        { isVisible ? "visibility" : "visibility_off" }
                    </span>
                </button>
            </div>

            { error && (
                <p className="text-[9px] font-bold uppercase tracking-wider px-1 text-secondary">
                    Error: { error }
                </p>
            ) }
        </div>
    );
}
