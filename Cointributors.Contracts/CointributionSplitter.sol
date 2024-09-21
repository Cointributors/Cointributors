// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

import "@openzeppelin/contracts/token/ERC20/IERC20.sol";
import "contracts/ICointributionSplitter.sol";

contract CointributionSplitter {

    // Struct to hold the recipient's information
    struct Recipient {
        address recipientAddress;
        uint256 portion;    // Portion of the donation (in basis points, 1% = 100)
        bool isSplitter;    // Whether the recipient is another CointributionSplitter contract
    }

    // Array to store all recipients
    Recipient[] public recipients;

    // Event to be emitted when a donation is made
    event DonationReceived(address indexed donor, address indexed token, uint256 amount);

    // Constructor to initialize recipients and their portions
    constructor(Recipient[] memory _recipients) {
        uint256 totalPortion = 0;
        for (uint256 i = 0; i < _recipients.length; i++) {
            require(_recipients[i].portion > 0, "Portion must be greater than zero");
            totalPortion += _recipients[i].portion;
            recipients.push(_recipients[i]);
        }

        // Ensure the total portions add up to 100% (or 10,000 basis points)
        require(totalPortion == 10000, "Total portion must equal 10,000");
    }

    // Donate function to distribute tokens to multiple recipients
    function donate(uint256 amount, address tokenAddress) public {
        require(amount > 0, "Donation must be greater than 0");

        IERC20 token = IERC20(tokenAddress); // Create an instance of the ERC20 token being donated

        for (uint256 i = 0; i < recipients.length; i++) {
            uint256 recipientShare = (amount * recipients[i].portion) / 10000;

            if (recipients[i].isSplitter) {
                // If the recipient is another CointributionSplitter, call its donate function
                ICointributionSplitter(recipients[i].recipientAddress).donate(recipientShare, tokenAddress);
            } else {
                // Otherwise, transfer the donation tokens to the recipient's wallet
                require(token.transferFrom(msg.sender, recipients[i].recipientAddress, recipientShare), "Token transfer failed");
            }
        }

        emit DonationReceived(msg.sender, tokenAddress, amount);
    }

    // Get details of a recipient (address, portion, and whether it is a CointributionSplitter)
    function getRecipient(uint256 index) public view returns (address, uint256, bool) {
        require(index < recipients.length, "Recipient index out of bounds");
        return (recipients[index].recipientAddress, recipients[index].portion, recipients[index].isSplitter);
    }
}
