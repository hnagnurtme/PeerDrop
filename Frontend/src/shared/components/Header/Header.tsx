import { SecondaryButton } from "../Button";

export default function Header () {

    return (
        <header className="w-full bg-surface border-b-4 border-black px-6 py-4 sticky top-0 z-50">
            <div className="flex items-center justify-between">
                {/* Logo */ }
                <div className="flex items-center gap-3">

                    <div className="size-10 bg-primary border-4 border-black shadow-neo-sm flex items-center justify-center">
                        <span className="material-symbols-outlined text-black font-bold">terminal</span>
                    </div>

                    <h1 className="text-2xl font-black tracking-tight uppercase text-black">PixelPort_</h1>

                </div>

                {/* Navigator */ }
                <nav className="hidden md:flex items-center gap-8">
                    <a className="text-black font-bold text-lg hover:underline decoration-4 decoration-primary underline-offset-4" href="File2.html">Dashboard</a>

                    <a className="text-black font-bold text-lg hover:underline decoration-4 decoration-primary underline-offset-4" href="File3.html">History</a>

                    <a className="text-black font-bold text-lg hover:underline decoration-4 decoration-primary underline-offset-4" href="File4.html">Settings</a>
                </nav>
                {/* Upgrade Button */ }
                <div className="flex items-center gap-4">
                    <SecondaryButton >
                            Upgrade
                    </SecondaryButton>
                </div>


            </div>
        </header>
    );
};