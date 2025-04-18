This project solves linear second-order ordinary differential equations (ODEs) of the form:  
\[ a_2(x)y'' + a_1(x)y' + a_0(x)y = b(x) \]  
using a hybrid numerical approach:  
1. **Gill's Method** (4th-order Runge-Kutta variant) for initial steps.  
2. **Milne's Predictor-Corrector Method** for subsequent integration.  