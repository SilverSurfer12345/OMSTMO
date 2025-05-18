# Delivery Charge Feature

This feature adds automatic delivery charge calculation to the POS system based on the customer's postcode.

## How It Works

1. When the "Delivery" button is clicked, the system automatically calculates a delivery charge based on the customer's postcode.
2. The delivery charge is displayed in the price panel.
3. The delivery charge is included in the total price calculation.

## Postcode-Based Pricing

The delivery charge is calculated based on the postcode area:

- **NE16**: £2.50 (Base charge - restaurant's area)
- **Nearby areas** (NE15, NE17, etc.): £3.00 - £4.50
- **Further areas** (NE1, NE2, etc.): £5.00 - £7.50
- **Maximum charge areas** (DH, SR, TS): £8.50 - £10.00

## Installation

The delivery charge functionality is automatically installed when the application starts.

## Usage

1. Enter the customer's details, including their address with postcode.
2. Click the "Delivery" button.
3. The delivery charge will be automatically calculated and displayed.
4. The total price will include the delivery charge.

## Files

- `DeliveryChargeCalculator.cs`: Calculates the delivery charge based on postcode
- `DeliveryChargeExtension.cs`: Adds the delivery charge UI to the POS form
- `DeliveryChargeInstaller.cs`: Installs the delivery charge functionality

## Implementation Details

The implementation uses extension methods to add the delivery charge functionality to the existing POS form without modifying the original code. This approach ensures that the feature can be easily maintained and updated in the future.

The delivery charge is calculated based on a predefined mapping of postcode areas to charges, which can be easily updated in the `DeliveryChargeCalculator.cs` file.