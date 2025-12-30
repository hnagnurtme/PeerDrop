import React from 'react';
import clsx from 'clsx';

interface TerminalLine {
    label: string;
    value: string;
    isHighlight?: boolean;
}

interface CyberBrandingProps {
    theme: 'login' | 'signup';
    bgText: string;
    statusLabel: string;
    title: React.ReactNode;
    terminalLines: TerminalLine[];
}

const CyberBranding: React.FC<CyberBrandingProps> = ( {
    theme,
    bgText,
    statusLabel,
    title,
    terminalLines
} ) => {
    const themeStyles = {
        login: {
            bg: 'bg-black',
            accent: 'text-primary',
            bgTextColor: 'text-white/5',
            statusBg: 'bg-primary',
            terminalBorder: 'border-primary/30',
        },
        signup: {
            bg: 'bg-secondary',
            accent: 'text-black',
            bgTextColor: 'text-white/10',
            statusBg: 'bg-black',
            terminalBorder: 'border-black/30',
        }
    };

    const styles = themeStyles[ theme ];

    return (
        <div className={ clsx(
            'hidden md:flex md:w-5/12 flex-col justify-between p-8 md:p-12 relative overflow-hidden',
            styles.bg
        ) }>
            {/* Large Background Text */ }
            <div className={ clsx(
                'absolute inset-0 flex items-center justify-center pointer-events-none select-none',
                styles.bgTextColor
            ) }>
                <span className="text-[20rem] font-black leading-none tracking-tighter">
                    { bgText }
                </span>
            </div>

            {/* Status Label */ }
            <div className="relative z-10">
                <div className={ clsx(
                    'inline-flex items-center gap-2 px-3 py-1.5 text-xs font-bold uppercase tracking-wider',
                    styles.statusBg,
                    theme === 'login' ? 'text-black' : 'text-white'
                ) }>
                    <span className="w-2 h-2 bg-current rounded-full animate-pulse" />
                    { statusLabel }
                </div>
            </div>

            {/* Main Title */ }
            <div className="relative z-10">
                <h1 className="text-5xl md:text-6xl lg:text-7xl font-black text-white uppercase leading-none tracking-tight">
                    { title }
                </h1>
            </div>

            {/* Terminal-style Info */ }
            <div className={ clsx(
                'relative z-10 border-2 p-4 font-mono text-xs',
                styles.terminalBorder
            ) }>
                <div className="space-y-2">
                    { terminalLines.map( ( line, index ) => (
                        <div key={ index } className="flex justify-between">
                            <span className="text-white/50">{ line.label }:</span>
                            <span className={ clsx(
                                line.isHighlight ? styles.accent : 'text-white'
                            ) }>
                                { line.value }
                            </span>
                        </div>
                    ) ) }
                </div>
            </div>
        </div>
    );
};

export default CyberBranding;
