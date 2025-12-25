import { createBrowserRouter } from "react-router"; 
import { lazy , Suspense } from "react";

import MainLayout from "@/app/layouts/Main.layout";

//Lazy load pages
const HomePage = lazy( () => import("@/features/home/pages/Home.page"))
const RoomPage = lazy( () => import("@/features/room/pages/Room.page"))

export const router = createBrowserRouter([
    {
        element : < MainLayout/>,
        children:  [
            {
                path : '/',
                element : (
                    <Suspense fallback={<div>Loading...</div>}>
                        <HomePage/>
                    </Suspense>
                )
            },
            {
                path : '/room/:roomId',
                element : (
                    <Suspense fallback={<div>Loading...</div>}>
                        <RoomPage/>
                    </Suspense>
                )
            }
        ]
    }
])


