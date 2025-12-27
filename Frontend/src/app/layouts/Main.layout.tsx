import { Outlet } from "react-router";
import { Header ,Footer} from "@/shared/components";

const MainLayout = () => {
    return (
        <div className="bg-background-light dark:bg-background-dark font-display min-h-screen flex flex-col overflow-x-hidden bg-grid-patter">
            {/* Header */}
            <Header />

            {/* Main  */}
            <main className="flex-1 container mx-auto px-4 py-6">
                <Outlet />
            </main>
            {/* Footer */ }
            <Footer/>
        </div>
    );
};

export default MainLayout;

