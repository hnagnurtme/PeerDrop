import { Outlet } from "react-router";
import { Header ,Footer} from "@/shared/components";

const MainLayout = () => {
    return (
        <div className="main-layout">
            {/* Add header, navigation, or other shared UI here */ }
            <Header/>

            <main>
                <Outlet />
            </main>
            {/* Add footer or other shared UI here */ }
            <Footer/>
        </div>
    );
};

export default MainLayout;
